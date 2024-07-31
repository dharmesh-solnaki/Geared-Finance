import { IGridSettings } from "./common-grid.model";

export class FundingCategory{
    constructor(
        public id:number,
        public name:string
    ){}
}
export class FundingEquipmentType{
    constructor(
        public name:string,
        public categoryId:number,
        public id?:number,
    ){}
}
// export class FundingEquipmentResponse{
//     constructor(
//         public id:number,
//         public name:string,
//         public catogory:FundingCategory
//     ){}
// }


// category
// : 
// {id: 1, name: 'Hospitality equipment'}
// id
// : 
// 2
// name
// : 
// "Pizza Oven"
export const FundingEquipmentTypeGridSetting:IGridSettings={
    columns: [
        {name:"name",title:"Funding/Equipment type",sort:true},
        {name:"categoryName", title:"Funding Category",sort:true}
    ],
    showPagination:true,
    pageSizeValues:[
      { pageNo: 10, text: '10 per pager' },
      { pageNo: 25, text: '25 per pager' },
      { pageNo: 50, text: '50 per pager' },
      { pageNo: 100, text: '100 per pager' },
    ]
}