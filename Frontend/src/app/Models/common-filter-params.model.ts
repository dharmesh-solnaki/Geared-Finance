export class FilterParams {
  constructor(
    public pipelineType: number,
    public leadType: number,
    public ownerIds: number[],
    public vendorId: number,
    public vendorSalesIds: number[],
    public recordType: string,
    public listType: number
  ) {}
}
