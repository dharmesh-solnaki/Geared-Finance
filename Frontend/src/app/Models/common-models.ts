
export class BaseRespons<T>{
   constructor(
    public responseData:T[],
    public totalRecords:number
   ){}
}

export class Vendor{
    constructor(
        public id:number,
        public name:string,
    ){}
}

export class IsExistData{   
    constructor(
       public isEmailExist:boolean,
       public isExistMobile:boolean

    ) {}
}
// export class FundingCategory{
//     constructor(
//         public id:number,
//         public name:string
//     ){}
// }
// export class FundingEquipmentType{
//     constructor(
//         public name:string,
//         public categoryId:number,
//         public id?:number,
//     ){}
// }

// export class FundingEquipmentResponse{
//     constructor(
//         public id:number,
//         public name:string,
//         public isBeingUsed:boolean,
//         public category:FundingCategory,
//         public categoryName?:string
//     ){}
// }