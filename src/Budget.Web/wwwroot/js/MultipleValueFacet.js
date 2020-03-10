import { facet } from "./Facets.js";

class multipleValueFacet extends facet {
    render() {
        const options = JSON.parse(this.props.options);
        const defaults = JSON.parse(this.props.value);
        const checked = (o) => defaults.find(x => x === o) ? "checked='checked'" : "";
        const items = (values) => values.map(x =>
            `<div><label><input type="checkbox" name="${props.name}" value="${x}" ${checked(x)} />${x}</label></div>`)
            .join("");

        return `
<div id="${this.id}" class="component facet mulitple-value">
    <h3>${this.props.title}</h3>
    ${items(options)}
</div>`;
    }

    // TODO: should return an array?
    //get value() {
    //    return document
    //        .querySelectorAll(`#${this.id} input[name="${this.props.name}"]:checked`)
    //        .value;
    //}
}

customElements.define('multiple-value-facet', multipleValueFacet);