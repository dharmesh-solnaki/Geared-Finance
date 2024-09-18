import { ColumnType, IGridSettings } from './common-grid.model';

export class Note {
  constructor(
    public id: number = 0,
    public createdDate: string = String.Empty,
    public userName: string = String.Empty,
    public description: string = String.Empty
  ) {}
}

export const NotesGridSetting: IGridSettings = {
  columns: [
    {
      name: 'createdDate',
      title: 'created date',
      sort: true,
      type: ColumnType.DATE,
    },
    { name: 'userName', title: 'user', sort: false },
    {
      name: 'description',
      title: 'notes',
      sort: false,
      type: ColumnType.NOTE,
    },
  ],
  showNoteEdit: true,
  showNoteDelete: true,
  showPagination: true,
  pageSizeValues: [
    { pageNo: 10, text: '10 per page' },
    { pageNo: 25, text: '25 per page' },
    { pageNo: 50, text: '50 per page' },
    { pageNo: 100, text: '100 per page' },
  ],
};
