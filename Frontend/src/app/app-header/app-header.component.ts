import {
  Component,
  EventEmitter,
  HostListener,
  OnInit,
  Output,
  TemplateRef,
} from '@angular/core';
import { menuBarItems } from '../Shared/constants';
import { TokenService } from '../Service/token.service';
import { RolePermissionService } from '../Service/role-permission.service';
import { SharedTemplateService } from '../Service/shared-template.service';
import { getRoleIdByRoleName } from '../Shared/common-functions';
import { HeaderSearchModel } from '../Models/common-models';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppHeaderComponent implements OnInit {
  menuData: { menuItem: string; imagePath: string; routerPath?: string }[] = [];
  shouldVisible: boolean = false;
  shouldVisibleCanvas: boolean = false;
  userName: string = 'User';
  isShowsettingHeader: boolean = false;
  visibleModulesList: string[] = [];
  headerSearchList: HeaderSearchModel[] = [];
  isShowSearchBar: boolean = true;
  headerSearchValue = String.Empty;
  selectedIndex: number = 0;
  isOverlayVisible: boolean = false;
  @Output() isNavOpned = new EventEmitter<boolean>();

  constructor(
    private _tokenService: TokenService,
    private _rolePermissionService: RolePermissionService,
    private _templateService: SharedTemplateService
  ) {}
  ngOnInit(): void {
    this.menuData = menuBarItems;
    this.setVisibleModuleList();
    this.headerListSetter();
  }

  get getHeaderTemplateRef(): TemplateRef<HTMLElement> | null {
    return this._templateService.getHeaderTemplate();
  }

  get getUsername(): string {
    return this._tokenService.getUserNameFromToken().split(' ')[0];
  }

  get getSearchStatus(): boolean {
    return this._templateService.isSearchRequired;
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
      this.shouldVisible = false;
    } else {
      this.shouldVisibleCanvas = false;
      this.shouldVisible = !this.shouldVisible;
    }
    this.isNavOpned.emit(this.shouldVisible);
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

  searchChangeHandler() {
    // if (this.headerSearchValue.trim()) {
    this._templateService.searchSubject.next(this.headerSearchValue.trim());
    // }
  }
  clearHeaderSearch() {
    this.headerSearchValue = String.Empty;
    this._templateService.searchList.next([]);
    this._templateService.searchSubject.next('-1');
    this._templateService.isSearchCleared.next(true);
    this.isOverlayVisible = false;
  }
  headerListSetter() {
    this._templateService.searchList.subscribe((res) => {
      this.headerSearchList = res;
    });
  }

  @HostListener('window:keydown', ['$event'])
  handleKeyDownEvent(event: KeyboardEvent): void {
    const itemCount = this.headerSearchList.length;
    if (itemCount > 0) {
      switch (event.key) {
        case 'ArrowDown':
          this.selectedIndex = (this.selectedIndex + 1) % itemCount;
          break;
        case 'ArrowUp':
          this.selectedIndex = (this.selectedIndex - 1 + itemCount) % itemCount;
          break;
        case 'Enter':
          if (this.headerSearchList[this.selectedIndex]) {
            const itemId = this.headerSearchList[this.selectedIndex].id;
            this.headerListClickHandler(itemId);
          }
          break;
      }
    }
  }

  headerListClickHandler(id: number) {
    this._templateService.searchedId.next(id);
    this.headerSearchValue = this.headerSearchList.filter(
      (x) => x.id == id
    )[0].name;
    this.headerSearchList = [];
    this.isOverlayVisible = false;
  }
  listHoverHandler(index: number) {
    this.selectedIndex = index;
  }
}
