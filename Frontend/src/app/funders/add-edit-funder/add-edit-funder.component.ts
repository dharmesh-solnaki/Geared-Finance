import { Component, HostListener, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonTransfer } from 'src/app/Models/common-transfer.model';
import { SharedTemplateService } from 'src/app/Service/shared-template.service';
import { PhonePipe } from 'src/app/Pipes/phone.pipe';
import { FunderService } from 'src/app/Service/funder.service';
import {
  csvMaker,
  getAddressFromApi,
  validateDocType,
} from 'src/app/Shared/common-functions';
import { CommonTransferComponent } from 'src/app/Shared/common-transfer/common-transfer.component';
import {
  AddressOptions,
  FunderModuleConstants,
  alertResponses,
  ckEditorConfig,
  selectMenu,
  statusSelectMenu,
  validationRegexes,
} from 'src/app/Shared/constants';
import { FunderProductGuideComponent } from '../funder-product-guide/funder-product-guide.component';
import { Note, NotesGridSetting } from 'src/app/Models/note.model';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
} from 'src/app/Models/common-grid.model';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { TokenService } from 'src/app/Service/token.service';
import { environment } from 'src/environments/environment.development';
import { FunderChartComponent } from '../funder-chart/funder-chart.component';

@Component({
  selector: 'app-add-edit-funder',
  templateUrl: './add-edit-funder.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class AddEditFunderComponent {
  activeTemplate: TemplateRef<HTMLElement> | null = null;
  activeTab: string = FunderModuleConstants.ACTIVE_OVERVIEW_TAB;
  selectMenuStatus: selectMenu[] = [];
  funderForm: FormGroup = new FormGroup({});
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  editorConfig: CKEDITOR.config = ckEditorConfig;
  funderId: number = 0;
  addressOptions = AddressOptions;
  isFormSubmitted: boolean = false;
  isEdit: boolean = false;
  entityName = 'Funder';
  funderLogoUrl: string = String.Empty;
  funderDbLogo: string = String.Empty;
  isFunderGuideFormChanged: boolean = false;
  @ViewChild('overViewTemplate', { static: true })
  overViewTemplate!: TemplateRef<HTMLElement>;
  @ViewChild('funderProductTypeGuide', { static: true })
  funderProductTypeGuide!: TemplateRef<HTMLElement>;
  @ViewChild('funderHeaderTabs', { static: true })
  funderHeaderTabs!: TemplateRef<HTMLElement>;
  @ViewChild('addEditFunderHeader', { static: true })
  addEditFunderHeader!: TemplateRef<HTMLElement>;
  @ViewChild('funderOverview', { static: true })
  funderOverview!: TemplateRef<HTMLElement>;
  @ViewChild('funderDocuments', { static: true })
  funderDocuments!: TemplateRef<HTMLElement>;
  @ViewChild('commonShareComponent', { static: false })
  commonShareComponent!: CommonTransferComponent;
  @ViewChild('funderProductGuide')
  funderProductGuide!: FunderProductGuideComponent;
  @ViewChild('funderInterestRateChart')
  funderInterestRateChart!: FunderChartComponent;

  // notes
  notesModalTitle: string = String.Empty;
  isSaveBtnDisabled: boolean = true;
  isAddBtnDisabled: boolean = false;
  noteDescription: string = String.Empty;
  selectedNoteId: number = -1;
  // disableSave: boolean = false;
  tempNote!: Note;
  noteList: Note[] = [];
  gridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;
  totalRecords: number = 0;
  searchingModel: CommonSearch = {
    pageNumber: 1,
    pageSize: 10,
  };

  constructor(
    private _templateService: SharedTemplateService,
    private _fb: FormBuilder,
    private _toaster: ToastrService,
    private _funderService: FunderService,
    private _route: ActivatedRoute,
    private _router: Router,
    private _tokenService: TokenService
  ) {
    this.isEdit = this._route.snapshot.params['id'] != undefined;
    if (this.isEdit) {
      const id = this._route.snapshot.params['id'];

      if (id) {
        this.funderId = id;
        this._funderService.getFunder(id).subscribe((res) => {
          res.bdmPhone = new PhonePipe().transform(res.bdmPhone);
          let domainName = res.bdmEmail.split('@')[1];
          // const externalLogoUrl = `https://img.logo.dev/${domainName}?token=${environment.LOGO_DEV_API}&size=50`;
          // this.funderLogoUrl = externalLogoUrl;
          this.funderForm.patchValue(res);

          this.funderDbLogo = `data:${res.imgType || 'image/png'};base64,${
            res.logoImg || String.Empty
          }`;
          this.entityName = res.entityName;
          this.notesModalTitle = `Notes | ${this.entityName.toUpperCase()}`;
        });
      }
    }
  }

  ngOnInit() {
    this.setActiveTab(this.activeTab, this.overViewTemplate);
    this._templateService.setTemplate(this.funderHeaderTabs);
    this._templateService.setHeaderTemplate(this.addEditFunderHeader);
    this.selectMenuStatus = statusSelectMenu;
    this.initializeFunderForm();
    if (this.isEdit) {
      this.gridSetting = NotesGridSetting;
      this.noteListSetter();
    }
  }

  initializeFunderForm() {
    this.funderForm = this._fb.group({
      name: [, [Validators.required]],
      abn: [
        ,
        [
          Validators.required,
          Validators.minLength(11),
          Validators.maxLength(11),
        ],
      ],
      status: [false, Validators.required],
      bank: [],
      bsb: [, [Validators.minLength(6), Validators.maxLength(6)]],
      account: [],
      streetAddress: [],
      suburb: [],
      state: [],
      postcode: [, [Validators.minLength(4)]],
      postalAddress: [],
      postalSuburb: [],
      postalState: [],
      postalPostcode: [, [Validators.minLength(4)]],
      creditAppEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      settlementsEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      adminEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      payoutsEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      collectionEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      eotEmail: [, [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      bdmName: [, [Validators.required]],
      bdmSurname: [, [Validators.required]],
      bdmEmail: [
        ,
        [
          Validators.required,
          Validators.pattern(validationRegexes.EMAIL_REGEX),
        ],
      ],
      bdmPhone: [String.Empty],
      id: [0],
      entityName: [],
    });
  }
  setActiveTab(tab: string, template: TemplateRef<HTMLElement>) {
    if (
      (this.funderForm.dirty &&
        this.activeTab === FunderModuleConstants.ACTIVE_OVERVIEW_TAB) ||
      (this.isFunderGuideFormChanged &&
        this.activeTab ===
          FunderModuleConstants.ACTIVE_FUNDER_PRODUCT_GUIDE_TAB)
    ) {
      const confirmLeave = confirm(alertResponses.UNSAVE_CONFIRMATION);
      if (!confirmLeave) {
        return;
      }
    }

    this.activeTab = tab;
    this.activeTemplate = template;
  }

  handleAddressChange(
    place: google.maps.places.PlaceResult,
    prefix: string
  ): void {
    const { address, postcode, suburb, state } = getAddressFromApi(place);
    if (prefix == 'SA') {
      this.funderForm.get('streetAddress')?.setValue(address);
      this.funderForm.get('suburb')?.setValue(suburb);
      this.funderForm.get('state')?.setValue(state);
      this.funderForm.get('postcode')?.setValue(postcode);
    } else {
      this.funderForm.get('postalAddress')?.setValue(address);
      this.funderForm.get('postalSuburb')?.setValue(suburb);
      this.funderForm.get('postalState')?.setValue(state);
      this.funderForm.get('postalPostcode')?.setValue(postcode);
    }
  }

  handleFormsubmit() {
    this.isFormSubmitted = true;
    if (
      this.isEdit &&
      this.activeTab === FunderModuleConstants.ACTIVE_FUNDER_PRODUCT_GUIDE_TAB
    ) {
      this.funderProductGuide.funderGuideFormSubmit();
    }

    if (this.activeTab === FunderModuleConstants.ACTIVE_OVERVIEW_TAB) {
      if (this.funderForm.invalid) {
        return;
      }

      const phonevalue = (
        this.funderForm.get('bdmPhone')?.value as string
      ).replaceAll(' ', String.Empty);
      this.funderForm.get('bdmPhone')?.setValue(phonevalue);

      this._funderService.upsertFunder(this.funderForm.value).subscribe(
        (res) => {
          const message =
            this.funderId !== 0
              ? alertResponses.UPDATE_RECORD
              : alertResponses.ADD_RECORD;
          this._toaster.success(message);
          this.funderId = res;
          this.funderForm.get('id')?.setValue(res);
          // this.funderGuideForm.get('funderId')?.setValue(res);
          this._router.navigate([`funder/${this.funderId}/Edit`]);
        },
        () => this._toaster.error(alertResponses.ERROR)
      );
    }
  }
  handleImgUpload(ev: Event) {
    const input = ev.target as HTMLInputElement;
    if (input.files && input.files.length > 0 && this.isEdit) {
      const file = input.files[0];
      const isValidFormat = validateDocType(file.type, 'img');
      if (isValidFormat) {
        const formData = new FormData();
        formData.append('logoImage', file);
        this._funderService.uploadImg(formData, this.funderId).subscribe(() => {
          this.funderLogoUrl = URL.createObjectURL(file);
        });
      } else {
        this._toaster.error(alertResponses.INVALID_DOC_TYPE_IMG);
      }
    }
  }
  handleLogoErr() {
    if (this.funderDbLogo) {
      this.funderLogoUrl = this.funderDbLogo;
    }
  }
  funderGuideFormchange(ev: boolean) {
    this.isFunderGuideFormChanged = ev;
  }
  SaveEntityName(field: HTMLInputElement) {
    const inputValue = field.value.trim();
    if (inputValue) {
      this.entityName = inputValue;
      this.funderForm.get('entityName')?.setValue(this.entityName);
      document.getElementById('cancelEntityModal')?.click();
    }
  }
  deleteFunder() {
    this._funderService.deleteFunder(this.funderId).subscribe(
      () => {
        this._toaster.success(alertResponses.DELETE_RECORD);

        this._router.navigate(['/funder']);
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
    document.getElementById('cancelFunderDelModal')?.click();
  }
  ngOnDestroy(): void {
    this._templateService.setTemplate(null);
    this._templateService.setHeaderTemplate(null);
  }

  @HostListener('window:beforeunload', ['$event'])
  canChangePage(event: BeforeUnloadEvent) {
    if (this.funderForm.dirty || this.isFunderGuideFormChanged) {
      return window.confirm(alertResponses.UNSAVE_CONFIRMATION);
    } else {
      return;
    }
  }

  onNoteAddition() {
    const userName = this._tokenService.getUserNameFromToken();
    this.tempNote = new Note(0, Date.now().toString(), userName, String.Empty);
    this.noteList = [...this.noteList, this.tempNote];
    this.isAddBtnDisabled = true;
  }
  onModalSaveClick() {
    this._funderService
      .upsertNote(this.funderId, this.tempNote)
      .subscribe((res) => {
        if (this.tempNote.id === 0) {
          this.tempNote.id = res;
        }
        this.resetNoteField();
      });
    this.paginationSetter();
  }
  onModalClose() {
    this.noteList = this.noteList.filter((x) => x.id !== 0);
    this.isAddBtnDisabled = false;
  }
  onNoteEdition(ev: Note) {
    this.selectedNoteId = ev.id;
    this.noteDescription = ev.description;
    this.tempNote = ev;
  }
  onNoteChange(ev: string) {
    this.tempNote.description = ev;
    this.noteDescription = ev;
    this.isSaveBtnDisabled = ev == String.Empty;
  }

  onNoteDeletion(ev: number) {
    if (ev > 0) {
      this._funderService.deleteNote(ev).subscribe();
    }
    this.noteList = this.noteList.filter((x) => x.id != ev);
    this.resetNoteField();
  }

  noteListSetter() {
    this._funderService
      .getNotes(this.searchingModel, this.funderId)
      .subscribe((res) => {
        if (res) {
          this.totalRecords = res.totalRecords;
          this.noteList = res.responseData;
        }
        this.paginationSetter();
      });
  }
  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.totalRecords,
      currentPage: this.searchingModel.pageNumber,
      selectedPageSize: [`${this.searchingModel.pageSize} per page`],
    };
  }
  pageChangeEventHandler(page: number) {
    this.searchingModel.pageNumber = page;
    this.noteListSetter();
  }
  pageSizeChangeHandler(pageSize: number) {
    if (this.totalRecords > pageSize) {
      this.searchingModel.pageNumber = 1;
      this.searchingModel.pageSize = pageSize;
      this.noteListSetter();
    }
  }
  sortHandler(ev: SortConfiguration) {
    const { sort, sortOrder } = ev;
    this.searchingModel.sortBy = sort.trim();
    this.searchingModel.sortOrder = sortOrder;
    this.searchingModel.pageNumber = 1;
    this.noteListSetter();
  }
  downloadNotes() {
    const csvTitle = `Funder diary notes - ${this.entityName}`;
    let downloadData: {
      CreatedDate: string;
      UserName: string;
      Note: string;
    }[] = [];

    const download = (data: Note[]) => {
      downloadData = data.map(({ createdDate, userName, description }) => ({
        CreatedDate: createdDate,
        UserName: userName,
        Note: description,
      }));
      csvMaker(downloadData, csvTitle);
    };

    if (this.noteList.length === this.totalRecords) {
      download(this.noteList);
    } else {
      const { pageNumber, pageSize } = this.searchingModel;
      this.searchingModel.pageSize = Number.INT_MAX_VALUE;
      this.noteListSetter();
      this._funderService
        .getNotes(this.searchingModel, this.funderId)
        .subscribe((res) => {
          download(res.responseData);
          this.searchingModel.pageNumber = pageNumber;
          this.searchingModel.pageSize = pageSize;
        });
    }
  }

  resetNoteField() {
    this.selectedNoteId = -1;
    this.noteDescription = String.Empty;
    this.tempNote = new Note();
    this.isAddBtnDisabled = false;
    this.isSaveBtnDisabled = true;
  }
}
