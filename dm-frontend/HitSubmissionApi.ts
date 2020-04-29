import {paging} from "./globals";
export class Api
{
    token: string;
    constructor(token: string){
        this.token = token;
    }
   async hitGetApi(url)
    {
        (document.getElementById("loading") as HTMLDivElement).style.display = "flex";    // loading start 
        let res = await fetch(url, {
            headers: new Headers({"Authorization": `Bearer ${this.token}`})
        });
        let metadata=JSON.parse(res.headers.get('X-Pagination'));
        paging(metadata);
        let data = await res.json();
        console.log(data);
        (document.getElementById("loading") as HTMLDivElement).style.display = "none";    //  loading end
        return await data;
    }
}