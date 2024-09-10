import { DecimalPipe } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {
  CommonTransfer,
  SubCategory,
} from 'src/app/Models/common-transfer.model';
import { EquipmentService } from 'src/app/Service/equipment.service';
import { FunderService } from 'src/app/Service/funder.service';
import { CommonTransferComponent } from 'src/app/Shared/common-transfer/common-transfer.component';
import { alertResponses, ckEditorConfig } from 'src/app/Shared/constants';

@Component({
  selector: 'app-funder-product-guide',
  templateUrl: './funder-product-guide.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
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
  @ViewChild('LtoRTransfer', { static: false })
  LtoRTransfer!: CommonTransferComponent;
  @ViewChild('RtoLTransfer', { static: false })
  RtoLTransfer!: CommonTransferComponent;
  funderId: number = 0;

  constructor(
    private _fb: FormBuilder,
    private _equipmentService: EquipmentService,
    private _decimalPipe: DecimalPipe,
    private _toaster: ToastrService,
    private _funderService: FunderService,
    private _route: ActivatedRoute
  ) {
    const id = this._route.snapshot.params['id'];

    if (id) {
      this.funderId = id;
      this._funderService.getFunderGuide(id).subscribe((res) => {
        res && this.funderGuideForm.patchValue(res);
        let tempData = this.funderGuideForm.get('selectedFundings')?.value;
        if (tempData) {
          this.exsitingListSetter(tempData);
        }
      });
    }
  }

  ngOnInit(): void {
    this.setActiveFunderTab(this.activeFunderTab, this.funderOverview);
    this.intializeFunderOverviewForm();
    this.equipmentDataSetter();
    // this.filterAvailableFunding();
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
  funderGuideFormSubmit() {
    this.checkValidityFunderGuide();
    console.log(this.funderGuideForm);
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
  checkValidityFunderGuide() {
    const funderGuideForm = this.funderGuideForm;
    const selctedFundings = funderGuideForm.get('selectedFundings')?.value;

    if (selctedFundings.length <= 0) {
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
        this.filterAvailableFunding();
      }
    });
  }
  exsitingListSetter(list: any) {
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
  addToSelectedList() {
    if (!this.LtoRTransfer.isTempListEmpty()) {
      this.LtoRTransfer.addToSelectedList();

      // Update 'existedFundings' with new subcategories
      this.LtoRTransfer.tempList.forEach((selectedItem) => {
        // Check if the category already exists in 'existedFundings'
        const existingCategory = this.existedFundings.find(
          (category) => category.id === selectedItem.id
        );

        if (existingCategory) {
          // Add missing subcategories only
          selectedItem.subCategory.forEach((newSubCat) => {
            const subCatExists = existingCategory.subCategory.some(
              (existingSubCat) => existingSubCat.id === newSubCat.id
            );
            if (!subCatExists) {
              existingCategory.subCategory.push(newSubCat);
            }
          });
        } else {
          // If the category doesn't exist, add it completely
          this.existedFundings.push(selectedItem);
        }
      });

      // Update 'availableFunding' to remove transferred subcategories
      this.LtoRTransfer.tempList.forEach((selectedItem) => {
        const availableCategory = this.availableFunding.find(
          (category) => category.id === selectedItem.id
        );

        if (availableCategory) {
          // Remove selected subcategories from the available list
          availableCategory.subCategory = availableCategory.subCategory.filter(
            (subCat) =>
              !selectedItem.subCategory.some(
                (selectedSubCat) => selectedSubCat.id === subCat.id
              )
          );

          // If all subcategories are removed, remove the category
          if (availableCategory.subCategory.length === 0) {
            this.availableFunding = this.availableFunding.filter(
              (category) => category.id !== availableCategory.id
            );
          }
        }
      });

      // Update the form field with the selected fundings
      this.funderGuideForm
        .get('selectedFundings')
        ?.setValue(this.existedFundings);

      // Clear the temporary list
      this.LtoRTransfer.clearTheTempList();
    }
  }

  removeFromSelectedList() {
    if (!this.RtoLTransfer.isTempListEmpty()) {
      this.RtoLTransfer.addToSelectedList();
      // Add the removed items back to the available list
      this.availableFunding = [
        ...this.availableFunding,
        ...this.RtoLTransfer.tempList.filter(
          (item) =>
            !this.availableFunding.some((existing) => existing.id === item.id)
        ),
      ];

      // Ensure that subcategories are correctly merged and avoid duplications
      this.RtoLTransfer.tempList.forEach((itemToAdd) => {
        const existingItem = this.availableFunding.find(
          (item) => item.id === itemToAdd.id
        );
        if (existingItem) {
          itemToAdd.subCategory.forEach((subCat) => {
            const subCatExists = existingItem.subCategory.some(
              (existingSubCat) => existingSubCat.id === subCat.id
            );
            if (!subCatExists) {
              existingItem.subCategory.push(subCat);
            }
          });
        } else {
          this.availableFunding.push(itemToAdd);
        }
      });

      // Update the selected funding list
      this.existedFundings = [...this.RtoLTransfer.availableDivDisplayList];
      this.funderGuideForm
        .get('selectedFundings')
        ?.setValue(this.existedFundings);

      // Clear the temporary list after processing
      this.RtoLTransfer.clearTheTempList();
    }
  }
  filterAvailableFunding() {
    this.availableFunding = this.availableFunding
      .map((funding) => {
        // Find the corresponding funding item in existedFundings
        const existingFunding = this.existedFundings.find(
          (ef) => ef.id === funding.id
        );

        if (existingFunding) {
          // Filter out the subCategories present in existedFunding
          const filteredSubCategories = funding.subCategory.filter(
            (subCat) =>
              !existingFunding.subCategory.some(
                (existingSubCat) => existingSubCat.id === subCat.id
              )
          );

          // Return a new CommonTransfer object with the filtered subCategories
          return new CommonTransfer(
            funding.id,
            funding.name,
            filteredSubCategories
          );
        }

        // If no matching existingFunding found, return the funding item as is
        return funding;
      })
      .filter((funding) => funding.subCategory.length > 0);
  }
}
