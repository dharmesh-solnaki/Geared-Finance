import { ColumnType, IGridSettings } from './common-grid.model';

export class FundingCategory {
  constructor(public id: number, public name: string) {}
}
export class FundingEquipmentType {
  constructor(
    public name: string,
    public categoryId: number,
    public id?: number
  ) {}
}
export class FundingEquipmentResponse {
  constructor(
    public id: number,
    public name: string,
    public isBeingUsed: boolean,
    public category: FundingCategory,
    public categoryName?: string
  ) {}
}

export const FundingEquipmentTypeGridSetting: IGridSettings = {
  columns: [
    {
      name: 'name',
      title: 'Funding/Equipment type',
      type: ColumnType.EQUIPMENTTYPE,
      sort: true,
    },
    { name: 'categoryName', title: 'Funding Category', sort: true },
  ],
  showPagination: true,
  showEquipmentTypeDelete: true,
  showEquipmentTypeSave: true,
  showEquipmentTypeEdit: true,
  pageSizeValues: [
    { pageNo: 10, text: '10 per page' },
    { pageNo: 25, text: '25 per page' },
    { pageNo: 50, text: '50 per page' },
    { pageNo: 100, text: '100 per page' },
  ],
};
