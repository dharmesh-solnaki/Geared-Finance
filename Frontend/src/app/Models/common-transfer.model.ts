export class CommonTransfer {
  constructor(
    public id: number,
    public name: string,
    public subCategory: SubCategory[]
  ) {}
}

export class SubCategory {
  constructor(public id: number, public name: string) {}
}
