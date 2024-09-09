import { DecimalPipe } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  CommonTransfer,
  SubCategory,
} from 'src/app/Models/common-transfer.model';
import { SharedTemplateService } from 'src/app/Models/shared-template.service';
import { PhonePipe } from 'src/app/Pipes/phone.pipe';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { FunderService } from 'src/app/Service/funder.service';
import { getAddressFromApi } from 'src/app/Shared/common-functions';
import { CommonTransferComponent } from 'src/app/Shared/common-transfer/common-transfer.component';
import {
  AddressOptions,
  alertResponses,
  ckEditorConfig,
  selectMenu,
  statusSelectMenu,
  validationRegexes,
} from 'src/app/Shared/constants';

@Component({
  selector: 'app-add-edit-funder',
  templateUrl: './add-edit-funder.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class AddEditFunderComponent {
  activeTemplate: TemplateRef<any> | null = null;
  activeTab: string = 'overview';
  activeFunderTemplate: TemplateRef<any> | null = null;
  activeFunderTab: string = 'funderOverview';
  selectMenuStatus: selectMenu[] = [];
  funderForm: FormGroup = new FormGroup({});
  funderGuideForm: FormGroup = new FormGroup({});
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  editorConfig: CKEDITOR.config = ckEditorConfig;
  funderId: number = 0;
  addressOptions = AddressOptions;
  isFormSubmitted: boolean = false;
  isEdit: boolean = false;
  @ViewChild('overViewTemplate', { static: true })
  overViewTemplate!: TemplateRef<any>;
  @ViewChild('funderProductTypeGuide', { static: true })
  funderProductTypeGuide!: TemplateRef<any>;
  @ViewChild('funderHeaderTabs', { static: true })
  funderHeaderTabs!: TemplateRef<any>;
  @ViewChild('funderOverview', { static: true })
  funderOverview!: TemplateRef<any>;
  @ViewChild('funderDocuments', { static: true })
  funderDocuments!: TemplateRef<any>;
  @ViewChild('commonShareComponent', { static: false })
  commonShareComponent!: CommonTransferComponent;

  constructor(
    private _templateService: SharedTemplateService,
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _decimalPipe: DecimalPipe,
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
          this.funderForm.patchValue(res);
        });
      }
    }
  }

  ngOnInit() {
    this.setActiveTab(this.activeTab, this.overViewTemplate);
    this.setActiveFunderTab(this.activeFunderTab, this.funderOverview);
    this._templateService.setTemplate(this.funderHeaderTabs);
    this.selectMenuStatus = statusSelectMenu;
    this.initializeFunderForm();
    this.intializeFunderOverviewForm();
    this.equipmentDataSetter();
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
    });
  }

  intializeFunderOverviewForm() {
    this.funderGuideForm = this._fb.group({
      financeType: ['', [Validators.required]],
      rates: [],
      isBrokerageCapped: [false],
      isApplyRitcFee: [false],
      ritcFee: [],
      isApplyAccountKeepingFee: [false],
      accountKeepingFee: [],
      isApplyDocumentFee: [false],
      funderDocFee: [],
      matrixNotes: [],
      generalNotes: [],
      eotNotes: [],
      cutoff: [],
      craa: [],
      selectedFundings: [],
      funderId: [this.funderId],
      id: [0],
    });
  }
  setActiveTab(tab: string, template: TemplateRef<any>) {
    this.activeTab = tab;
    this.activeTemplate = template;
    if (this.isEdit && tab == 'funderProductGuide') {
      this._funderService.getFunderGuide(this.funderId).subscribe((res) => {
        res && this.funderGuideForm.patchValue(res);
        let tempData = this.funderGuideForm.get('selectedFundings')?.value;
        if (tempData) {
          this.exsitingListSetter(tempData);
        }
      });
      setTimeout(() => {
        if (this.commonShareComponent) {
          this.commonShareComponent.filterExistedList();
        }
      });
    }
  }

  setActiveFunderTab(tab: string, template: TemplateRef<any>) {
    this.activeFunderTab = tab;
    this.activeFunderTemplate = template;
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
    if (this.isEdit && this.activeTab === 'funderProductGuide') {
      this.checkValidityFunderGuide();
      if (this.funderGuideForm.invalid) {
        this.funderGuideForm.markAllAsTouched();
        let errorMsg = alertResponses.ON_FORM_INVALID;
        const invalidFields = Object.keys(this.funderGuideForm.controls)
          .filter((field) => this.funderGuideForm.get(field)?.invalid)
          .map((field) => `<br> - ${field}`);

        this._toaster.error(`${errorMsg} ${invalidFields}`, '', {
          enableHtml: true,
        });
        return;
      }

      const funderFormType = this.funderGuideForm.value;
      const id = this.funderGuideForm.get('id')?.value;

      this._funderService.upsertFunderGuide(funderFormType).subscribe(
        (res) => {
          this._toaster.success(
            id ? alertResponses.UPDATE_RECORD : alertResponses.ADD_RECORD
          );
          this.funderGuideForm.get('id')?.setValue(res);
        },
        () => this._toaster.error(alertResponses.ERROR)
      );
    }

    if (this.activeTab === 'overview') {
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
          this.funderGuideForm.get('funderId')?.setValue(res);
          this._router.navigate([`funder/${this.funderId}/Edit`]);
          // this.isEdit = true;
        },
        () => this._toaster.error(alertResponses.ERROR)
      );
    }
  }

  equipmentDataSetter() {
    const searchModel: CommonSearch = {
      pageNumber: 1,
      pageSize: Number.INT_MAX_VALUE,
    };
    this._equipmentService.getEquipmentTypes(searchModel).subscribe((res) => {
      if (res && res.responseData) {
        const groupedData: { [key: number]: CommonTransfer } = {};

        res.responseData.forEach((item) => {
          const categoryId = item.category.id;
          if (!groupedData[categoryId]) {
            groupedData[categoryId] = new CommonTransfer(
              categoryId,
              item.category.name,
              []
            );
          }
          groupedData[categoryId].subCategory.push(
            new SubCategory(item.id, item.name)
          );
        });
        this.availableFunding = Object.values(groupedData);
      }
    });
  }

  exsitingListSetter(list: any) {
    console.log(list);
    const groupedData: { [key: number]: CommonTransfer } = {};

    list.forEach((item: any) => {
      const categoryId = item.id;
      if (!groupedData[categoryId]) {
        groupedData[categoryId] = new CommonTransfer(categoryId, item.name, []);
      }
      item.subCategory.forEach((subCat: any) => {
        groupedData[categoryId].subCategory.push(
          new SubCategory(subCat.id, subCat.name)
        );
      });
    });
    this.existedFundings = Object.values(groupedData);
  }

  selectedFundingList(ev: CommonTransfer[]) {
    if (ev.length > 0) {
      this.funderGuideForm.get('selectedFundings')?.setValue(ev);
    } else {
      this.funderGuideForm.get('selectedFundings')?.reset();
    }
  }
  handleInputChange(field: string, type: number) {
    const formField = this.funderGuideForm.get(field);
    if (!formField?.value) {
      formField?.setValue('0.00');
    } else {
      let cleanedInput = formField?.value.replace(/[^\d.]/g, '');
      const dotIndex = cleanedInput.indexOf('.');
      if (dotIndex !== -1) {
        cleanedInput =
          cleanedInput.substring(0, dotIndex + 1) +
          cleanedInput.substring(dotIndex + 1).replace(/\./g, '');
      }
      let result2 =
        type == 1
          ? this._decimalPipe.transform(cleanedInput, '1.2-2') || '0.00'
          : this._decimalPipe.transform(cleanedInput, '1.0-2');

      formField?.setValue(result2);
    }
  }
  handleCheckoxChange(type: number) {
    const financeType = this.funderGuideForm.get('financeType');
    const financeTypeValue = financeType?.value || '';
    const selectedType = type === 0 ? 'Chattel mortgage' : 'Rental';
    const hasType = financeTypeValue.includes(selectedType);

    let updatedValue: string;
    if (hasType) {
      updatedValue = financeTypeValue
        .replace(selectedType, '')
        .replace(/,\s*,/g, ',')
        .replace(/^\s*,|,\s*$/g, '')
        .trim();
    } else {
      updatedValue = financeTypeValue
        ? `${financeTypeValue}, ${selectedType}`.trim()
        : selectedType;
    }
    financeType?.setValue(updatedValue);
    console.log(financeType?.value);
  }

  checkValidityFunderGuide() {
    const funderGuideForm = this.funderGuideForm;
    if (!funderGuideForm.get('selectedFundings')?.value) {
      funderGuideForm
        .get('selectedFundings')
        ?.setValidators(Validators.required);
    } else {
      funderGuideForm.get('selectedFundings')?.clearValidators();
    }
    funderGuideForm.get('selectedFundings')?.updateValueAndValidity();

    if (funderGuideForm.get('isApplyRitcFee')?.value) {
      if (!funderGuideForm.get('ritcFee')?.value) {
        funderGuideForm.get('ritcFee')?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('ritcFee')?.clearValidators();
    }
    funderGuideForm.get('ritcFee')?.updateValueAndValidity();

    if (funderGuideForm.get('isApplyAccountKeepingFee')?.value) {
      const monthlyFee = funderGuideForm.get('accountKeepingFee')?.value;
      if (!monthlyFee) {
        funderGuideForm
          .get('accountKeepingFee')
          ?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('accountKeepingFee')?.clearValidators();
    }
    funderGuideForm.get('accountKeepingFee')?.updateValueAndValidity();
    if (funderGuideForm.get('isApplyDocumentFee')?.value) {
      const monthlyFee = funderGuideForm.get('funderDocFee')?.value;
      if (!monthlyFee) {
        funderGuideForm.get('funderDocFee')?.setValidators(Validators.required);
      }
    } else {
      funderGuideForm.get('funderDocFee')?.clearValidators();
    }
    funderGuideForm.get('funderDocFee')?.updateValueAndValidity();
  }
}
