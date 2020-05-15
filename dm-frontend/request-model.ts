// @ts-nocheck
import { BASEURL, navigationBarsss, amIUser,headersRows } from "./globals";
(async function(){
    const Select = mdc.select.MDCSelect;
    const TextField = mdc.textField.MDCTextField;
    let { id, token }=JSON.parse(localStorage.getItem("user_info"));
     let role = await amIUser(token) == true ? "User" : "Admin";
  
    class RequestDevice
    {
        userId: number
        deviceModel : string
        deviceBrand : string
        deviceType : string
        specs : Specification
        noOfDays : number = 15
        comment : string = ""
    }
    class Specification
    {
        ram : String 
        storage : String
        screenSize : String
        connectivity :  String
    }

    console.log(Select);

    // const MDCFooFoundation = mdc.select.MDCSelectFoundation;
    const selects = [].map.call(document.querySelectorAll('.mdc-select'), function(el) {
        
        return new Select(el);
    });

    console.log(selects);
    const textField = new TextField(document.querySelector('.mdc-text-field'));


    // document.addEventListener('onload' ,e =>)


    // select.listen('MDCSelect:change', ev => {
    //     console.log('Value : ' + select.value + " at index " + select.selectedIndex);
    // })


    function submitForm()
    {
        const url = BASEURL + "/api/request/add"
        let data = fun()
        console.log(data);
        
        fetch(url , {
            method : "post",
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(data)
        }).then(res =>
            console.log(res))
            .catch(e =>
                console.log(e));
                

    }

    function initialiseDropdowns(){
    const url = BASEURL + '/api/dropdown/';
    

        // Iterate over all the dropdown elements
        selects.forEach(selectElement => {
            const urlSnip = selectElement.root_.dataset.urlSnip;  // Gets the `data-url-snip` attribute and add it with the url to make a fetch request
            
            fetch(url + urlSnip,{ headers: {
                'Authorization': `Bearer ${token}`
            }})
            .then(res => res.json())
            .then(data => populateDropdown(urlSnip, data));
        });

    }


    function  fun () :  RequestDevice
    {
        let specs = new Specification();
        let request = new RequestDevice();
        request.userId = parseInt(id);
        request.deviceType = selects[0].value;
        request.deviceBrand = selects[1].value;
        request.deviceModel = selects[2].value;
        specs.ram = selects[3].value;
        specs.storage = selects[4].value;
        specs.screenSize = selects[5].value;
        specs.connectivity = selects[6].value;
        request.specs =  specs;
        return request;
    }

    function populateDropdown(id : string, dataArray: any)
    {
        console.log(dataArray);
        clear(id);
        for(let data of dataArray)
        {
            document.getElementById(id).innerHTML += `<li class="mdc-list-item" data-value="${data["name"]}">
                                                        <span class="mdc-list-item__text">
                                                        ${data["name"]}
                                                        </span>
                                                        </li>`
        }
    }


    function clear(id : string)
    {

        document.getElementById(id).innerHTML = '';
    }


initialiseDropdowns();
navigationBarsss(role,"navigation");
headersRows(role,"row1");
document.querySelector('button').addEventListener('click', function(){submitForm()});

})();
