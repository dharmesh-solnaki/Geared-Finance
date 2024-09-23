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
import {
  Subject,
  Subscription,
  debounceTime,
  distinctUntilChanged,
  of,
  switchMap,
  takeUntil,
} from 'rxjs';

@Component({
  selector: 'app-funders',
  templateUrl: './funders.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class FundersComponent {
  @ViewChild('funderDefaultHeader', { static: true })
  funderDefaultHeader!: TemplateRef<HTMLElement>;
  funderList: Funder[] = [];
  gridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;
  totalRecords: number = 0;
  searchingModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };
  isEnableLoader: boolean = false;
  ngUnsubscribe = new Subject<void>();
  isResetFunderList: boolean = true;
  funderHeaderListSubscription = new Subscription();
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
    this._templateService.isSearchRequired = true;
    this.getFunderSearchData();
    this.getFilteredFunders();
    this.getFunderListOnClearSearch();
  }
  onEditEventRecevier(id: number) {
    this.isResetFunderList = false;
    this._templateService.isSearchCleared.next(true);
    this._templateService.searchedId.next(0);
    this._templateService.searchList.next([]);
    this._templateService.searchSubject.next(String.Empty);
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
    this.isResetFunderList = false;
    this._templateService.setTemplate(null);
    this._templateService.isSearchRequired = false;
    this.funderHeaderListSubscription.unsubscribe();
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  pageChangeEventHandler(page: number) {
    this.searchingModel.pageNumber = page;
    this.funderListSetter();
  }
  pageSizeChangeHandler(pageSize: number) {
    if (this.totalRecords > pageSize) {
      this.searchingModel.pageNumber = 1;
      this.searchingModel.pageSize = pageSize;
      this.funderListSetter();
    }
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
  getFunderSearchData() {
    this.funderHeaderListSubscription = this._templateService.searchSubject
      .pipe(
        takeUntil(this.ngUnsubscribe),
        debounceTime(600),
        distinctUntilChanged(),
        // filter((search: string) => !!search.trim()),
        switchMap((search) => {
          if (search === '-1') {
            return of([]);
          } else {
            return this._funderService.getFunderSearch(search);
          }
        })
      )
      .subscribe((res) => {
        res && this._templateService.searchList.next(res);
      });
  }
  getFilteredFunders() {
    this._templateService.searchedId
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        if (res > 0) {
          this.funderList = this.funderList.filter((x) => x.id == res);
          if (this.funderList.length == 0) {
            this.searchingModel.id = res;
            this.searchingModel.pageNumber = 1;
            this.searchingModel.pageSize = 1;
            this.funderListSetter();
          }
        }
      });
  }
  getFunderListOnClearSearch() {
    this._templateService.isSearchCleared
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((res) => {
        if (res && this.isResetFunderList) {
          this.searchingModel.id = undefined;
          this.searchingModel.pageSize = 10;
          this.funderListSetter();
          this.isResetFunderList = true;
        }
      });
  }
}
