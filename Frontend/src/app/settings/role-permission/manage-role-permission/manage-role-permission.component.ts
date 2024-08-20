import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { RolePermissionService } from 'src/app/Service/role-permission.service';
import {
  ModuleType,
  RolePermissionDTO,
} from 'src/app/Models/role-management.model';
import { FormControl, FormGroup } from '@angular/forms';
import { firstValueFrom } from 'rxjs';
import { TokenService } from 'src/app/Service/token.service';
import { ToastrService } from 'ngx-toastr';
import { alertResponses } from 'src/app/Shared/constants';

@Component({
  selector: 'app-manage-role-permission',
  templateUrl: './manage-role-permission.component.html',
  styleUrls: ['../../../Shared/common-grid/common-grid.component.css'],
})
export class ManageRolePermissionComponent implements OnInit {
  roleName: string = '';
  moduleList: ModuleType[] = [];
  rolePermissionList: RolePermissionDTO[] = [];
  rightsForm: FormGroup = new FormGroup({});
  roleId: number = 0;
  isEdit: boolean = false;
  searchModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 1,
  };
  constructor(
    private _route: ActivatedRoute,
    private _rolePermissionService: RolePermissionService,
    private _tokenService: TokenService,
    private _toasterService: ToastrService
  ) {}

  ngOnInit(): void {
    this.roleId = this._route.snapshot.params['id'];
    this.searchModel.id = this.roleId;
    this._rolePermissionService.getRoles(this.searchModel).subscribe((res) => {
      if (res) {
        this.roleName = res[0].roleName;
      }
    });
    this.formInitializer();
  }

  async formInitializer() {
    this.rolePermissionList = await firstValueFrom(
      this._rolePermissionService.getRigthsForEdit(this.roleId)
    );
    this.moduleList = await firstValueFrom(
      this._rolePermissionService.getModules()
    );

    if (this.rolePermissionList) {
      this.isEdit = true;

      this.rolePermissionList.forEach((e) => {
        const moduleName = e.module?.moduleName.replaceAll(' ', '');
        this.rightsForm.addControl(
          `${moduleName}View`,
          new FormControl(e.canView)
        );
        this.rightsForm.addControl(
          `${moduleName}Add`,
          new FormControl(e.canAdd)
        );
        this.rightsForm.addControl(
          `${moduleName}Edit`,
          new FormControl(e.canEdit)
        );
        this.rightsForm.addControl(
          `${moduleName}Delete`,
          new FormControl(e.canDelete)
        );
      });
    } else {
      this.moduleList &&
        this.moduleList.map((e) => {
          const moduleName = e.moduleName.replaceAll(' ', '');

          ['View', 'Add', 'Edit', 'Delete'].forEach((action) => {
            this.rightsForm.addControl(
              `${moduleName}${action}`,
              new FormControl(false)
            );
          });
        });
    }
  }
  rightsFormHandler() {
    this.setRightPermissionList();
    console.log(this.rolePermissionList);
    this._rolePermissionService
      .upsertRolePermissions(this.rolePermissionList)
      .subscribe(
        () => {
          this.isEdit
            ? this._toasterService.success(alertResponses.UPDATE_RECORD)
            : this._toasterService.success(alertResponses.ADD_RECORD);
        },
        () => this._toasterService.error(alertResponses.ERROR)
      );
  }

  getFomControlName(item: ModuleType, type: string) {
    return `${item.moduleName.replaceAll(' ', '')}${type}`;
  }

  setViewControlValidity(moduleName: string, type: string) {
    const form = this.rightsForm;
    if (form.get(`${moduleName.replaceAll(' ', '')}${type}`)?.value) {
      form.get(`${moduleName.replaceAll(' ', '')}View`)?.setValue(true);
    }
  }

  setOtherControlValidity(moduleName: string) {
    const form = this.rightsForm;
    if (!form.get(`${moduleName.replaceAll(' ', '')}View`)?.value) {
      ['Add', 'Edit', 'Delete'].forEach((action) => {
        form.get(`${moduleName.replace(' ', '')}${action}`)?.setValue(false);
      });
    }
  }
  setRightPermissionList() {
    const token = this._tokenService.getDecryptedToken();
    const userId = +token.userId;
    const form = this.rightsForm;

    if (this.isEdit) {
      this.rolePermissionList.map((e) => {
        ['View', 'Add', 'Edit', 'Delete'].forEach((action) => {
          const propName = `can${action}` as keyof RolePermissionDTO;
          (e[propName] as boolean) = this.rightsForm.get(
            this.getFomControlName(e.module as ModuleType, action)
          )?.value;
        });
        e.userId = userId;
      });
    } else {
      this.rolePermissionList = [];
      this.moduleList.forEach((e) => {
        let rights = new RolePermissionDTO(
          0,
          e.id,
          +this.roleId,
          form.get(this.getFomControlName(e, 'View'))?.value,
          form.get(this.getFomControlName(e, 'Add'))?.value,
          form.get(this.getFomControlName(e, 'Edit'))?.value,
          form.get(this.getFomControlName(e, 'Delete'))?.value,
          userId
        );
        this.rolePermissionList.push(rights);
      });
    }
  }
}
