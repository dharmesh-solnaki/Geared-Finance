import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  OnInit,
  Output,
  TemplateRef,
} from '@angular/core';
import { menuBarItems } from '../Shared/constants';
import { TokenService } from '../Service/token.service';
import { RolePermissionService } from '../Service/role-permission.service';
import { SharedTemplateService } from '../Models/shared-template.service';
import { getRoleIdByRoleName } from '../Shared/common-functions';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppHeaderComponent implements OnInit {
  menuData: { menuItem: string; imagePath: string; routerPath?: string }[] = [];
  shuoldVisible: boolean = false;
  shouldVisibleCanvas: boolean = false;
  userName: string = 'User';
  isShowsettingHeader: boolean = false;
  visibleModulesList: string[] = [];
  @Output() isNavOpned = new EventEmitter<boolean>();

  constructor(
    private _tokenService: TokenService,
    private _rolePermissionService: RolePermissionService,
    private _templateService: SharedTemplateService,
    private _cdRef: ChangeDetectorRef
  ) {}
  ngOnInit(): void {
    this.menuData = menuBarItems;
    this.setVisibleModuleList();
  }

  get getTemplateRef(): TemplateRef<any> | null {
    return this._templateService.getTemplate();
  }
  get getUsername(): string {
    return this._tokenService.getUserNameFromToken();
  }

  get isShowSettingHeaderSection(): void {
    const element = document.getElementById('settingHeaderTop');
    if (window.innerWidth < 900) {
      element?.classList.remove('d-lg-flex');
      element?.classList.add('d-none');
    } else {
      element?.classList.add('d-lg-flex');
      element?.classList.remove('d-none');
    }
    return;
  }

  navExpand() {
    const windowWidth = window.innerWidth;
    if (windowWidth <= 768) {
      this.shouldVisibleCanvas = true;
      this.shuoldVisible = false;
    } else {
      this.shouldVisibleCanvas = false;
      this.shuoldVisible = !this.shuoldVisible;
    }
    this.isNavOpned.emit(this.shuoldVisible);
  }
  onLogout() {
    this._tokenService.clearToken();
  }

  checkModuleVisibility(moduleName: string) {
    return this.visibleModulesList.includes(moduleName.toLowerCase());
  }

  setVisibleModuleList() {
    const roleId = getRoleIdByRoleName(
      this._tokenService.getDecryptedToken().userRole
    );
    this.visibleModulesList = [];
    this._rolePermissionService.getRigthsForEdit(roleId).subscribe((res) => {
      if (res) {
        res.map((e) => {
          !e.canView &&
            this.visibleModulesList.push(
              e.module?.moduleName.toLowerCase() as string
            );
        });
      }
    });
  }
}
