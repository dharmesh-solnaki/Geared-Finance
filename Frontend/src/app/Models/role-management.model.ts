import { IGridSettings } from './common-grid.model';

export class RoleModel {
  constructor(
    public id: number,
    public roleName: string,
    public hasEditRights: boolean
  ) {}
}
export const roleGridSetting: IGridSettings = {
  columns: [{ name: 'roleName', title: 'Name', sort: true }],
  showRolePermissionEdit: true,
  showPagination: false,
  isShowIndex: true,
};

export class ModuleType {
  constructor(public moduleName: string, public id: number) {}
}

export class RolePermissionDTO {
  constructor(
    public id: number,
    public moduleId: number,
    public roleId: number,
    public canView: boolean,
    public canAdd: boolean,
    public canEdit: boolean,
    public canDelete: boolean,
    public userId: number,
    public module?: ModuleType
  ) {}
}
