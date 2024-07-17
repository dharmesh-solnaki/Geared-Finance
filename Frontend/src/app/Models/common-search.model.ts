import { SortOrder } from "./common-grid.model";

export class CommonSearch {

    constructor(
    public pageNumber:number,
    public pageSize:number,
    public sortBy?:string,
    public sortOrder?:SortOrder,
    public id?:number,
    public name?:string,
    public roleName?:string
    ) {}
}
