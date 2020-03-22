export class facet extends HTMLElement {
    _id = 0;

    constructor() {
        super();

        if (typeof facet.counter === "undefined") facet.counter = 0;

        this._id = facet.counter++;
    }

    connectedCallback() { this.innerHTML = this.render(); }

    render() { throw "render not implemented by derived class"; }

    options() {
        const raw = this.props.options;

        try {
            const f = Function('"use strict"; return (' + raw + ')')();
            const result = f();

            if (Array.isArray(result) && result.length > 0) return result;
        }
        catch (e) { }

        try {
            const json = JSON.parse(raw);
            return json;
        }
        catch (e) { }

        const literal = raw.split(",");

        if (Array.isArray(literal) && literal.length > 0) return literal;

        throw "options could not be understood. Expected a comma separated list, JSON array or a function that returns an array.";
    }

    get id() { return `${this.props.name}_${this._id}`; }
    get value() { throw "value not implemented by derived class"; }
    get name() { return this.props.name; }
    get props() {
        const obj = {};

        for (let i = 0; i < this.attributes.length; i++) {
            const p = this.attributes[i];
            obj[p.name] = p.value;
        }

        return obj;
    }
}
