import { facet } from "./Facets.js";

class multipleValueFacet extends facet {
    render() {
        const defaults = JSON.parse(this.props.value);
        const checked = (o) => defaults.find(x => x === o) ? "checked='checked'" : "";
        const items = (values) => values.map(x =>
            `<div><label><input type="checkbox" name="${this.props.name}" value="${x}" ${checked(x)} />${x}</label></div>`)
            .join("");

        return `
<div id="${this.id}" class="component facet mulitple-value">
    <h3>${this.props.title}</h3>
    ${items(this.options())}
</div>`;
    }

    
    get value() {
        var result = "";

        document
            .querySelectorAll(`#${this.id} input[name="${this.props.name}"]:checked`)
            .forEach(x => result += `${x.value},`);

        return result;
    }
}

customElements.define('multiple-value-facet', multipleValueFacet);