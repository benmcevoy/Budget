export class facet extends HTMLElement {
    _id = 0;

    constructor() {
        super();

        if (typeof facet.counter === "undefined") facet.counter = 0;

        this._id = facet.counter++;
    }

    connectedCallback() { this.innerHTML = this.render(); }
    render() { throw "render not implemented by derived class"; }
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
