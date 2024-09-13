import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonTransfer } from 'src/app/Models/common-transfer.model';
import { SharedTemplateService } from 'src/app/Service/shared-template.service';
import { PhonePipe } from 'src/app/Pipes/phone.pipe';
import { FunderService } from 'src/app/Service/funder.service';
import {
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

@Component({
  selector: 'app-add-edit-funder',
  templateUrl: './add-edit-funder.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class AddEditFunderComponent {
  activeTemplate: TemplateRef<any> | null = null;
  activeTab: string = FunderModuleConstants.ACTIVE_OVERVIEW_TAB;
  selectMenuStatus: selectMenu[] = [];
  funderForm: FormGroup = new FormGroup({});
  // funderGuideForm: FormGroup = new FormGroup({});
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  editorConfig: CKEDITOR.config = ckEditorConfig;
  funderId: number = 0;
  addressOptions = AddressOptions;
  isFormSubmitted: boolean = false;
  isEdit: boolean = false;
  entityName = 'Funder';
  funderLogoUrl: string = '';
  funderDbLogo: string = '';
  isFunderGuideFormChanged: boolean = false;
  @ViewChild('overViewTemplate', { static: true })
  overViewTemplate!: TemplateRef<any>;
  @ViewChild('funderProductTypeGuide', { static: true })
  funderProductTypeGuide!: TemplateRef<any>;
  @ViewChild('funderHeaderTabs', { static: true })
  funderHeaderTabs!: TemplateRef<any>;
  @ViewChild('addEditFunderHeader', { static: true })
  addEditFunderHeader!: TemplateRef<any>;
  @ViewChild('funderOverview', { static: true })
  funderOverview!: TemplateRef<any>;
  @ViewChild('funderDocuments', { static: true })
  funderDocuments!: TemplateRef<any>;
  @ViewChild('commonShareComponent', { static: false })
  commonShareComponent!: CommonTransferComponent;
  @ViewChild('funderProductGuide')
  funderProductGuide!: FunderProductGuideComponent;

  constructor(
    private _templateService: SharedTemplateService,
    private _fb: FormBuilder,
    private _toaster: ToastrService,
    private _funderService: FunderService,
    private _route: ActivatedRoute,
    private _router: Router
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
            res.logoImg || ''
          }`;
          this.entityName = res.name;
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
      bdmPhone: [''],
      id: [0],
      entityName: [],
    });
  }
  setActiveTab(tab: string, template: TemplateRef<any>) {
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
      ).replaceAll(' ', '');
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
}
