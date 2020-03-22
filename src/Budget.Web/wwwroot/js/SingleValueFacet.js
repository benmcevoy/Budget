import { facet } from "./Facets.js";

class singleValueFacet extends facet {
    render() {
        const checked = (o) => o === this.props.value ? "checked='checked'" : "";
        const items = (values) => values.map(x =>
            `<div><label><input type="radio" name="${this.props.name}" ${checked(x)} value="${x}" />${x}</label></div>`)
            .join("");

        return `
<div id="${this.id}" class="component facet single-value">
    <h3>${this.props.title}</h3>
    ${items(this.options())}
</div>`;
    }

    get value() {
        return document
            .querySelector(`#${this.id} input[name="${this.props.name}"]:checked`)
            .value;
    }
}

customElements.define('single-value-facet', singleValueFacet);
