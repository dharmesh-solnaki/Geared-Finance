import { IGridSettings } from './common-grid.model';

export class Document {
  constructor(
    public id: number = 0,
    public fileName: string,
    public createdDate: string
  ) {}
}

export const documentGridSetting: IGridSettings = {
  columns: [
    { name: 'fileName', title: 'File Name', sort: true },
    { name: 'createdDate', title: 'Date Added', sort: true },
  ],
  showDelete: true,
  showDownload: true,
  showDocument: true,
  showPagination: true,
  pageSizeValues: [
    { pageNo: 10, text: '10 per pager' },
    { pageNo: 25, text: '25 per pager' },
    { pageNo: 50, text: '50 per pager' },
    { pageNo: 100, text: '100 per pager' },
  ],
};
