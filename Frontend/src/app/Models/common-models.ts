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