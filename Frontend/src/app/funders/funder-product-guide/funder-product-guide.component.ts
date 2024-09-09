import { DecimalPipe } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  CommonTransfer,
  SubCategory,
} from 'src/app/Models/common-transfer.model';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { FunderService } from 'src/app/Service/funder.service';
import { ckEditorConfig } from 'src/app/Shared/constants';

@Component({
  selector: 'app-funder-product-guide',
  templateUrl: './funder-product-guide.component.html',
  styles: [],
})
export class FunderProductGuideComponent {
  funderGuideForm: FormGroup = new FormGroup({});
  editorConfig: CKEDITOR.config = ckEditorConfig;
  activeFunderTemplate: TemplateRef<any> | null = null;
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  activeFunderTab: string = 'funderOverview';
  @ViewChild('funderOverview', { static: true })
  funderOverview!: TemplateRef<any>;
  @ViewChild('funderDocuments', { static: true })
  funderDocuments!: TemplateRef<any>;
  funderId: number = 0;

  constructor(
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _decimalPipe: DecimalPipe,
    private _toaster: ToastrService,
    private _funderService: FunderService
  ) {}

  ngOnInit(): void {
    this.setActiveFunderTab(this.activeFunderTab, this.funderOverview);
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
  setActiveFunderTab(tab: string, template: TemplateRef<any>) {
    this.activeFunderTab = tab;
    this.activeFunderTemplate = template;
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
  selectedFundingList(ev: CommonTransfer[]) {
    if (ev.length > 0) {
      this.funderGuideForm.get('selectedFundings')?.setValue(ev);
    } else {
      this.funderGuideForm.get('selectedFundings')?.reset();
    }
  }
}
