import { Component, TemplateRef, ViewChild } from '@angular/core';
import { SharedTemplateService } from '../Service/shared-template.service';
import { CommonSearch } from '../Models/common-search.model';
import { FunderGridSettings, Funder } from '../Models/funder.model';
import { FunderService } from '../Service/funder.service';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
} from '../Models/common-grid.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-funders',
  templateUrl: './funders.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class FundersComponent {
  @ViewChild('funderDefaultHeader', { static: true })
  funderDefaultHeader!: TemplateRef<any>;
  funderList: Funder[] = [];
  gridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;
  totalRecords: number = 0;
  searchingModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };
  isEnableLoader: boolean = false;
  constructor(
    private _templateService: SharedTemplateService,
    private _funderService: FunderService,
    private _router: Router,
    private _route: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this._templateService.setTemplate(this.funderDefaultHeader);
    this.gridSetting = FunderGridSettings;
    this.funderListSetter();
  }
  onEditEventRecevier(id: number) {
    this._router.navigate([`${id}/Edit`], { relativeTo: this._route });
  }
  sortHandler(ev: SortConfiguration) {
    let { sort, sortOrder } = ev;
    if (sort == 'legalName') {
      sort = 'name';
    }
    if (sort == 'funder') {
      sort = 'enityName';
    }
    this.searchingModel.sortBy = sort.trim();
    this.searchingModel.sortOrder = sortOrder;
    this.searchingModel.pageNumber = 1;
    this.funderListSetter();
  }

  ngOnDestroy(): void {
    this._templateService.setTemplate(null);
  }
  pageChangeEventHandler(page: number) {
    this.searchingModel.pageNumber = page;
    this.funderListSetter();
  }
  pageSizeChangeHandler(pageSize: number) {
    this.searchingModel.pageNumber = 1;
    this.searchingModel.pageSize = pageSize;
    this.funderListSetter();
  }

  funderListSetter() {
    this.funderList = [];
    this.isEnableLoader = true;
    this._funderService.getFunders(this.searchingModel).subscribe(
      (res) => {
        if (res && res.responseData) {
          this.totalRecords = res.totalRecords;
          this.funderList = res.responseData;
        }
        this.paginationSetter();
        this.isEnableLoader = false;
      },
      () => (this.isEnableLoader = false)
    );
  }
  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.totalRecords,
      currentPage: this.searchingModel.pageNumber,
      selectedPageSize: [`${this.searchingModel.pageSize} per page`],
    };
  }
}
