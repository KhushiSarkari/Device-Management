import { BASEURL } from "../globals";
export class RolePermission{
    token: string;
    roles;
    permissions;
    mapping;
    
    constructor(token: string){
        this.token = token;
    }

    getRolesAndPermissions(){
        return fetch(BASEURL + "/api/rolepermission", {
            headers: {
                Authorization: `Bearer ${this.token}`
            }
        })
        .then(response => response.json())
        .then(mappingArray => mappingArray);
    }

    renderTable(){
        const parser = new DOMParser();
        let htmlString = parser.parseFromString(`
            <table class="mdl-data-table mdl-js-data-table mdl-shadow--2dp">
                <tr>
                    <td></td>
                    ${
                        this.permissions.reduce((acc, permissionsObject) => 
                            acc + `<th scope="col">${permissionsObject["PermissionName"]}</th>`
                        , '')
                    }
                </tr>
                ${
                    this.mapping.reduce((acc, roleObject, rowIdx) => 
                        {
                            return acc + `
                            <tr>
                                <th scope="row">
                                    ${roleObject["RoleName"]}
                                </th>
                                ${
                                    this.permissions.reduce((acc, permissionObject, colIdx) => 
                                        acc + `
                                            <td class="mdl-data-table__cell" onmouseenter="mouseOverTD(this);" onmouseleave="mouseOutTD(this);">
                                                <label class="mdl-checkbox mdl-js-checkbox" for="checkbox-${rowIdx}-${colIdx}">
                                                    <input type="checkbox" id="checkbox-${rowIdx}-${colIdx}" class="mdl-checkbox__input" ${roleObject["Permissions"] && roleObject["Permissions"].find(perm => perm["PermissionName"] == permissionObject["PermissionName"]) ? 'checked': ''}>
                                                    <span class="mdl-checkbox__label"></span>
                                                </label>
                                            </td>`
                                    , '')
                                }
                            </tr>`;
                        }, ''
                    )
                }
            </table>
        `, 'text/html');
        document.querySelector('#fixed-tab-1 .mdl-spinner').classList.remove("is-active");
        emptyElement(document.querySelector('#fixed-tab-1 .main'));
        document.querySelector('#fixed-tab-1 .main').appendChild(htmlString.body.firstChild);
        window["componentHandler"].upgradeDom();
    }

    checkboxListener(event: MouseEvent){
        const checkbox = event.target as HTMLInputElement;
        const rowIndex = checkbox.closest('tr').rowIndex - 1;
        const colIndex = checkbox.closest('td').cellIndex - 1;
        if(checkbox.checked){
            const PermissionToAdd = Object.assign({}, this.permissions[colIndex]);
            if(this.mapping[rowIndex].hasOwnProperty("Permissions"))
                this.mapping[rowIndex]["Permissions"].push(PermissionToAdd);
            else
                this.mapping[rowIndex]["Permissions"] = new Array(PermissionToAdd);
        }
        else{
            const PermissionToRemove = this.permissions[colIndex];
            const idxToDelete = this.mapping[rowIndex]["Permissions"].findIndex(obj => obj["PermissionName"] == PermissionToRemove["PermissionName"]);
            this.mapping[rowIndex]["Permissions"].splice(idxToDelete, 1);
        }
    }
    setup(){
        this.getRolesAndPermissions().then(mappingArray => {
            this.mapping = Array.from(mappingArray["Roles"]);
            this.roles = Object.keys(this.mapping);
            this.permissions = mappingArray.Permissions;
            this.renderTable();
        });
    }
    save(){
        fetch(BASEURL + "/api/rolepermission/update", {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${this.token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify({"Roles": this.mapping})
        }).then(response => {
            if(!response.ok)
                throw "";
            alert("Changes saved successfully");
            this.setup();
        }).catch(err => console.error(err));
    }
}

function mouseOverTD(tdElement){
    const rowTH = tdElement.parentElement.firstElementChild as HTMLTableHeaderCellElement;
    const columnTH = tdElement.parentElement.parentElement.firstElementChild.children[tdElement.cellIndex] as HTMLTableHeaderCellElement;
    rowTH.classList.add("activate");
    columnTH.classList.add("activate");
}
function mouseOutTD(tdElement){
    const rowTH = tdElement.parentElement.firstElementChild as HTMLTableHeaderCellElement;
    const columnTH = tdElement.parentElement.parentElement.firstElementChild.children[tdElement.cellIndex] as HTMLTableHeaderCellElement;
    rowTH.classList.remove("activate");
    columnTH.classList.remove("activate");
}
function emptyElement(element: HTMLElement){
    while(element.firstChild){
        element.removeChild(element.firstChild);
    }
}
window["mouseOverTD"] = mouseOverTD;
window["mouseOutTD"] = mouseOutTD;

