import { SortOrder } from './common-grid.model';

export class CommonSearch {
  constructor(
    public pageNumber: number = 1,
    public pageSize: number = 10,
    public sortBy?: string,
    public sortOrder?: SortOrder,
    public id?: number,
    public name?: string,
    public roleName?: string
  ) {}
}
