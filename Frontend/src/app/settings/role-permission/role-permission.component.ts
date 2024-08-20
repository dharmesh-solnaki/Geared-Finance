import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  IGridSettings,
  SortConfiguration,
} from 'src/app/Models/common-grid.model';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  RoleModel,
  roleGridSetting,
} from 'src/app/Models/role-management.model';
import { RolePermissionService } from 'src/app/Service/role-permission.service';
import { RoleEnum } from 'src/app/Shared/constants';
@Component({
  selector: 'app-role-permission',
  templateUrl: './role-permission.component.html',
})
export class RolePermissionComponent {
  searchModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };
  gridSetting!: IGridSettings;
  roleList: RoleModel[] = [];
  forbiddenRoles: string[] = [RoleEnum.VendorGuestUser];
  constructor(
    private _rolePermissionService: RolePermissionService,
    private _router: Router,
    private _route: ActivatedRoute
  ) {
    this.gridSetting = roleGridSetting;
    this.setRoleList();
  }

  onEditEventRecevier(id: number) {
    this._router.navigate([`${id}`], {
      relativeTo: this._route,
    });
  }

  sortHandler(ev: SortConfiguration) {
    const { sort, sortOrder } = ev;
    this.searchModel.sortBy = sort.trim();
    this.searchModel.sortOrder = sortOrder;
    this.searchModel.pageNumber = 1;
    this.setRoleList();
  }
  setRoleList() {
    this.roleList = [];
    this._rolePermissionService.getRoles(this.searchModel).subscribe((res) => {
      if (res) {
        this.roleList = res;
        this.roleList.map((e) => {
          e.hasEditRights = !this.forbiddenRoles.includes(e.roleName);
        });
      }
    });
  }
}
