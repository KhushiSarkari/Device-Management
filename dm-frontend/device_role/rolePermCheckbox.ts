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
                        this.roles.reduce((acc, roleObject) => 
                            acc + `<th scope="col">${roleObject["roleName"]}</th>`
                        , '')
                    }
                </tr>
                ${
                    this.permissions.reduce((acc, permissionObject, rowIdx) => 
                        {
                            return acc + `
                            <tr>
                                <th scope="row" class="td-underlines">
                                    ${permissionObject["permissionName"]}
                                </th>
                                ${
                                    this.mapping.reduce((acc, roleObject, colIdx) => 
                                        acc + `
                                            <td class="mdl-data-table__cell">
                                                <label class="mdl-checkbox mdl-js-checkbox" for="checkbox-${rowIdx}-${colIdx}">
                                                    <input type="checkbox" id="checkbox-${rowIdx}-${colIdx}" class="mdl-checkbox__input" ${roleObject["permissions"] && roleObject["permissions"].find(perm => perm["permissionName"] == permissionObject["permissionName"]) ? 'checked': ''}>
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
            const PermissionToAdd = Object.assign({}, this.permissions[rowIndex]);
            if(this.mapping[colIndex].hasOwnProperty("Permissions"))
                this.mapping[colIndex]["Permissions"].push(PermissionToAdd);
            else
                this.mapping[colIndex]["Permissions"] = new Array(PermissionToAdd);
        }
        else{
            const PermissionToRemove = this.permissions[rowIndex];
            const idxToDelete = this.mapping[colIndex]["Permissions"].findIndex(obj => obj["PermissionName"] == PermissionToRemove["PermissionName"]);
            this.mapping[colIndex]["Permissions"].splice(idxToDelete, 1);
        }
    }
    setup(){
        this.getRolesAndPermissions().then(mappingArray => {
            this.mapping = Array.from(mappingArray["roles"]);
            this.roles = this.mapping.map(({roleId, roleName}) => {return {roleId, roleName}});
            this.permissions = mappingArray.permissions;
            console.dir(this);
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
            if(!response.ok){
                throw "";}
                window["tata"].text('Changes Saved ','Successfully!',{duration:3000});
            this.setup();
        }).catch(err => console.error(err));
    }
}

function emptyElement(element: HTMLElement){
    while(element.firstChild){
        element.removeChild(element.firstChild);
    }
}
