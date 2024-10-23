import { DecimalPipe } from '@angular/common';
import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonTransfer } from 'src/app/Models/common-transfer.model';
import {
  InterestChart,
  RateChartOption,
} from 'src/app/Models/rateChartOption.model';
import { FunderService } from 'src/app/Service/funder.service';
import {
  addListToSelectedList,
  existingListSetter,
  filterAvailableFundingList,
} from 'src/app/Shared/common-functions';
import { CommonTransferComponent } from 'src/app/Shared/common-transfer/common-transfer.component';
import {
  FunderChartConstants,
  FunderModuleConstants,
  alertResponses,
} from 'src/app/Shared/constants';
import { arrayNotEmptyValidator } from 'src/app/Shared/validators/array-not-empty.validator';

@Component({
  selector: 'app-funder-chart',
  templateUrl: './funder-chart.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class FunderChartComponent {
  currentTabIndex: number = 0;
  chartArray: { id: number; name: string }[] = [{ id: 0, name: 'equipment 1' }];
  currentForm!: FormGroup;
  chartForms: FormGroup[] = [];
  availableFunding: CommonTransfer[] = [];
  existedFundings: CommonTransfer[] = [];
  // isShowVaryField: boolean = false;
  isShowChattelTerm: boolean = false;
  isShowRentalTerm: boolean = false;
  isShowChattelOption: boolean = false;
  isShowRentalOption: boolean = false;
  dbFinanceType: string = String.Empty;
  termTitle: string = FunderChartConstants.TERM_DEFAULT;
  termChartTitle: string = FunderChartConstants.TERM_RATE_DEFAULT;
  chosenFundingTitle: string = FunderModuleConstants.CHOSEN_FUNDING_TITLE;
  isFormSubmitted: boolean = false;
  isValidForm = false;
  @ViewChild('LtoRTransfer', { static: false })
  LtoRTransfer!: CommonTransferComponent;
  @ViewChild('RtoLTransfer', { static: false })
  RtoLTransfer!: CommonTransferComponent;
  funderId: number = 0;
  isChartForEdit: boolean = false;
  equipmentSubTabName = String.Empty;
  lastUpdateDateForChattel: Date = new Date();
  lastUpdateDateForRental: Date = new Date();
  isOnlyRentalType: boolean = false;
  @Output() isFunderChartFormDirty = new EventEmitter<boolean>();

  constructor(
    private _fb: FormBuilder,
    private _decimalPipe: DecimalPipe,
    private _toaster: ToastrService,
    private _funderService: FunderService,
    private _route: ActivatedRoute
  ) {
    const id = this._route.snapshot.params['id'];

    if (id) {
      this.funderId = id;
      this.getInterestChart();
    }
  }

  ngOnInit(): void {
    this.initializeCurrentForm();
  }

  initializeCurrentForm() {
    this.currentForm = this.createChartForm();
    this.chartForms.push(this.currentForm);
  }

  createChartForm(): FormGroup {
    return this._fb.group({
      id: [0],
      equipmentChartName: ['equipment 1', [Validators.required]],
      selectedFunding: [[] as CommonTransfer[]],
      typeOfFinance: [String.Empty, [Validators.required]],
      isInterestRatesVary: [false],
      chattelMortgageTerms: [[] as string[], [arrayNotEmptyValidator()]],
      rentalTerms: [[] as string[]],
      interestChartChattelMortgage: new FormArray([this.createInterestChart()]),
      interestChartRental: new FormArray([this.createInterestChart()]),
    });
  }
  createInterestChart(): FormGroup {
    return this._fb.group({
      id: [0],
      minValue: ['1,000'],
      maxValue: [],
      maxBrokerage: [],
      defaultRateAdjustment: [0.5],
      maxBrokerageCeiling: [],
    });
  }

  get interstChattelFormArr(): FormArray {
    return this.currentForm.get(
      FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE
    ) as FormArray;
  }

  get interestRentalFormArr(): FormArray {
    return this.currentForm.get(
      FunderChartConstants.INTEREST_CHART_RENTAL
    ) as FormArray;
  }

  addNewChart() {
    this.isChartForEdit;
    const lastForm = this.chartForms[this.chartForms.length - 1] as FormGroup;

    if (lastForm.valid) {
      const newId = -(this.chartArray.length + 10);
      this.chartArray.push({
        id: newId,
        name: `equipment ${this.chartArray.length + 1}`,
      });

      const newChartForm = this.createChartForm();
      this.currentTabIndex = newId;

      newChartForm.get('id')?.setValue(this.currentTabIndex);
      this.chartForms.push(newChartForm);

      this.currentForm = newChartForm;
      if (this.dbFinanceType.includes('Chattel mortgage')) {
        this.handleCheckboxChange(0);
      }
      if (this.dbFinanceType.includes('Rental')) {
        this.handleCheckboxChange(1);
      }
      this.currentForm
        .get('equipmentChartName')
        ?.setValue(`equipment ${this.chartArray.length}`);

      this.isFormSubmitted = false;
      this.existedFundings = [];
    }
  }
  switchChartTab(index: number) {
    this.chartForms.map((chartForm) => {
      if (chartForm.get('id')?.value == index) {
        this.currentForm = chartForm as FormGroup;
      }
    });
    // this.isFormSubmitted = false;
    this.currentTabIndex = index;
    this.validateInterestCharts();
    this.existedFundings = this.currentForm.get('selectedFunding')?.value;
    this.validateInterestCharts();
    this.updateVisibilityFlags();
  }
  handleCheckboxChange(type: number) {
    const financeType = this.currentForm.get('typeOfFinance');
    const financeTypeValue = (financeType?.value || String.Empty) as string;
    const selectedType = type === 0 ? 'Chattel mortgage' : 'Rental';
    const hasType = financeTypeValue.includes(selectedType);

    let updatedValue: string;
    if (hasType) {
      updatedValue = financeTypeValue
        .replace(selectedType, String.Empty)
        .replace(/,\s*,/g, ',')
        .replace(/^\s*,|,\s*$/g, String.Empty)
        .trim();
    } else {
      updatedValue = financeTypeValue
        ? `${financeTypeValue}, ${selectedType}`.trim()
        : selectedType;
    }
    financeType?.setValue(updatedValue);
    this.updateVisibilityFlags();
  }

  updateVisibilityFlags() {
    const fieldValue =
      this.currentForm.get('typeOfFinance')?.value || String.Empty;
    this.isShowChattelTerm = fieldValue.includes('Chattel mortgage');
    this.isShowRentalTerm = fieldValue.includes('Rental');

    // this.currentForm
    //   .get('isInterestRatesVary')
    //   ?.setValue(this.isShowChattelTerm && this.isShowRentalTerm);
    // this.currentForm
    //   .get('isInterestRatesVary')
    //   ?.setValue();

    this.ratesVaryChangeHandler();
  }
  ratesVaryChangeHandler() {
    const isInterestRatesVary = this.currentForm.get(
      'isInterestRatesVary'
    )?.value;
    if (this.isShowChattelTerm && this.isShowRentalTerm) {
      if (isInterestRatesVary) {
        this.termTitle = FunderChartConstants.TERM_CHATTEL_MORTGAGE;
        this.termChartTitle = FunderChartConstants.TERM_RATE_CHATTEL_MORTGAGE;
      } else {
        this.termTitle = FunderChartConstants.TERM_CHATTEL_RENTAL;
        this.termChartTitle = FunderChartConstants.TERM_RATE_CHATTEL_RENTAL;
      }
    } else if (this.isShowChattelTerm || this.isShowRentalTerm) {
      this.termTitle = FunderChartConstants.TERM_DEFAULT;
      this.termChartTitle = FunderChartConstants.TERM_RATE_DEFAULT;
    }
  }
  handleTermsChange(monthDuration: string, financeType: number) {
    const financeTerm =
      // financeType === 1 ? 'chattelMortgageTerms' : 'rentalTerms';
      financeType === 1
        ? FunderChartConstants.CHATTEL_MORTGAGE_TERMS
        : FunderChartConstants.RENTAL_TERMS;

    var currentTerms = this.currentForm.get(financeTerm)?.value as string[];
    let termArray =
      financeType === 1
        ? this.interstChattelFormArr
        : this.interestRentalFormArr;
    if (currentTerms && currentTerms.includes(monthDuration)) {
      currentTerms = currentTerms.filter((x) => x !== monthDuration);

      for (let index = 0; index < termArray.length; index++) {
        const formGroup = termArray.at(index) as FormGroup;
        formGroup.removeControl(`month${+monthDuration}`);
      }
    } else {
      for (let index = 0; index < termArray.length; index++) {
        const formGroup = termArray.at(index) as FormGroup;
        formGroup.addControl(`month${+monthDuration}`, new FormControl());
      }
      currentTerms.push(monthDuration);
    }
    currentTerms.sort();
    this.currentForm.get(financeTerm)?.setValue(currentTerms);
  }

  handleChartValueFormat(field: string, type: number, chartType: number) {
    const formFieldType =
      // chartType === 1 ? 'interestChartChattelMortgage' : 'interestChartRental';
      chartType === 1
        ? FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE
        : FunderChartConstants.INTEREST_CHART_RENTAL;
    const formArray = this.currentForm.get(formFieldType) as FormArray;

    const formField = formArray.at(formArray.length - 1).get(field);

    if (!formField) return;
    let cleanedInput = formField.value || String.Empty;
    cleanedInput = cleanedInput.replace(/[^\d.]/g, String.Empty);
    const dotIndex = cleanedInput.indexOf('.');
    if (dotIndex !== -1) {
      cleanedInput =
        cleanedInput.substring(0, dotIndex + 1) +
        cleanedInput.substring(dotIndex + 1).replace(/\./g, String.Empty);
    }
    const formattedValue = this._decimalPipe.transform(
      cleanedInput,
      type === 1 ? '1.0-0' : '1.0-2'
    );
    formField.setValue(formattedValue);
  }
  validateMinValueField(
    currChart: AbstractControl,
    formGroup: FormGroup | null,
    index: number,
    type: number
  ) {
    if (formGroup == null) {
      formGroup = this.currentForm;
    }
    const prevIndex = index - 1;
    if (prevIndex < 0) return;
    const termChartType =
      type === 1
        ? FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE
        : FunderChartConstants.INTEREST_CHART_RENTAL;

    const chartArray = formGroup.get(termChartType) as FormArray;

    const prevChart = chartArray.at(prevIndex) as FormGroup;
    const pervMaxVal = this.cleanFieldValue(prevChart.get('maxValue')?.value);
    let currMinValue = this.cleanFieldValue(currChart.get('minValue')?.value);

    if (+currMinValue <= +pervMaxVal) {
      this._toaster.error(
        `The Min value (${currMinValue}) added in line ${
          index + 1
        } can not be inferior to the Max value (${pervMaxVal}) on that line ${index}`
      );
      currChart.get('minValue')?.setErrors({ invalidMinValue: true });
    } else {
      currChart.get('minValue')?.setErrors(null);
    }
  }
  validateMaxValueField(
    currChart: AbstractControl,
    index: number,
    type: number
  ) {
    const chartRate = currChart as FormGroup;
    let minValue = this.cleanFieldValue(currChart.get('minValue')?.value);
    let maxValue = this.cleanFieldValue(
      currChart.get('maxValue')?.value || '0'
    );
    if (+maxValue <= +minValue) {
      this._toaster.error(
        `The Max value (${maxValue}) added in line ${
          index + 1
        } can not be inferior to the Min value (${minValue}) on that line`
      );
      chartRate.get('maxValue')?.setErrors({ invalidMaxValue: true });
    } else {
      chartRate.get('maxValue')?.setErrors(null);
    }
  }
  AddNewRateChart(chartRateType: number) {
    const chartType =
      chartRateType === 1
        ? FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE
        : FunderChartConstants.INTEREST_CHART_RENTAL;
    const chartArray = this.currentForm.get(chartType) as FormArray;
    const prevChart = chartArray.at(chartArray.length - 1) as FormGroup;
    this.validateMaxValueField(prevChart, chartArray.length - 1, chartRateType);
    if (prevChart.invalid) return;

    let maxValue = this.cleanFieldValue(prevChart.get('maxValue')?.value) + 1;
    const newRateChart = this._fb.group({});

    Object.keys(prevChart.controls).forEach((controlName) => {
      let controlValue = prevChart.get(controlName)?.value;
      if (controlName == 'id') {
        controlValue = 0;
      }
      if (controlName !== 'maxValue') {
        newRateChart.addControl(controlName, this._fb.control(controlValue));
      } else {
        newRateChart.addControl(controlName, new FormControl());
      }
    });
    let formattedMaxVal = this._decimalPipe.transform(
      maxValue,
      '1.0-0'
    ) as string;
    newRateChart.get('minValue')?.setValue(formattedMaxVal as never);
    chartArray.push(newRateChart);
  }

  removeRateChart(type: number) {
    let chartArray =
      type === 1 ? this.interstChattelFormArr : this.interestRentalFormArr;

    chartArray.removeAt(chartArray.length - 1);
  }

  copyChattelRateToRental() {
    const rentalRates = this.currentForm.get(
      FunderChartConstants.INTEREST_CHART_RENTAL
    ) as FormArray;

    const chattelArray = this.interstChattelFormArr;
    const chattelTerms = this.currentForm.get(
      FunderChartConstants.CHATTEL_MORTGAGE_TERMS
    )?.value;

    this.currentForm
      .get(FunderChartConstants.RENTAL_TERMS)
      ?.setValue(chattelTerms);

    while (rentalRates.length !== 0) {
      rentalRates.removeAt(0);
    }

    for (let index = 0; index < chattelArray.length; index++) {
      const formGroup = chattelArray.at(index) as FormGroup;
      const newFormGroup = this._fb.group({});
      Object.keys(formGroup.controls).forEach((controlName) => {
        let controlValue = formGroup.get(controlName)?.value;
        if (controlName == 'id') {
          controlValue = 0;
        }
        newFormGroup.addControl(controlName, this._fb.control(controlValue));
      });
      rentalRates.push(newFormGroup);
    }
  }
  cleanFieldValue(value: string | null): number {
    return +(value || '0').replaceAll(',', String.Empty);
  }
  addToSelectedList() {
    if (!this.LtoRTransfer.isTempListEmpty()) {
      this.LtoRTransfer.addToSelectedList();

      this.existedFundings = addListToSelectedList(
        this.LtoRTransfer.tempList,
        this.availableFunding,
        this.existedFundings
      );

      this.currentForm.get('selectedFunding')?.setValue(this.existedFundings);

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
      this.currentForm.get('selectedFunding')?.setValue(this.existedFundings);

      // Clear the temporary list after processing
      this.RtoLTransfer.clearTheTempList();
    }
  }
  filterAvailableFunding() {
    this.availableFunding = filterAvailableFundingList(
      this.availableFunding,
      this.existedFundings
    );
  }

  getInterestChart() {
    this._funderService.getFunderCharts(this.funderId).subscribe((res) => {
      this.isChartForEdit = false;
      if (res) {
        this.availableFunding = existingListSetter(res.availableFundings);
        this.dbFinanceType = res.typeOfFinance;

        if (this.dbFinanceType.includes('Chattel mortgage')) {
          this.isShowChattelOption = true;
          this.handleCheckboxChange(0);
        }
        if (this.dbFinanceType.includes('Rental')) {
          this.isShowRentalOption = true;
          this.handleCheckboxChange(1);
        }

        this.currentForm.get('typeOfFinance')?.setValue(res.typeOfFinance);
        if (res.rateCharts.length > 0) {
          this.chartForms = [];
          this.chartArray = [];
          res.rateCharts.forEach((element) => {
            const chartForm = this.createChartForm();
            chartForm.patchValue(element);

            let isChattelType =
              element.typeOfFinance.includes('Chattel mortgage');
            let isRentalType = element.typeOfFinance.includes('Rental');

            this.isShowChattelTerm = isChattelType;
            this.isShowRentalTerm = isRentalType;

            this.isShowChattelTerm &&
              this.processChartTerms(
                chartForm,
                isChattelType,
                element.chattelMortgageTerms,
                element.interestChartChattelMortgage,
                FunderChartConstants.CHATTEL_MORTGAGE_TERMS,
                FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE,
                1
              );

            this.isShowRentalOption &&
              this.processChartTerms(
                chartForm,
                isRentalType,
                element.rentalTerms,
                element.interestChartRental,
                FunderChartConstants.RENTAL_TERMS,
                FunderChartConstants.INTEREST_CHART_RENTAL,
                2
              );

            if (isRentalType && !isChattelType) {
              this.isOnlyRentalType = true;
              chartForm
                .get('chattelMortgageTerms')
                ?.setValue(element.rentalTerms.split(',').sort());

              const chattelMortgageArray = chartForm.get(
                'interestChartChattelMortgage'
              ) as FormArray;

              while (chattelMortgageArray.length > 0) {
                chattelMortgageArray.removeAt(0);
              }
              this.populateFormArray(
                chattelMortgageArray,
                element.interestChartRental,
                chartForm.get('chattelMortgageTerms')?.value
              );

              const rentalArray = chartForm.get(
                'interestChartRental'
              ) as FormArray;

              while (rentalArray.length > 0) {
                rentalArray.removeAt(0);
              }
              chartForm.get('rentalTerms')?.setValue([]);
              rentalArray.push(this.createInterestChart());
            }

            this.existedFundings = existingListSetter(element.selectedFunding);
            this.filterAvailableFunding();

            this.chartForms.push(chartForm);
            this.chartArray.push({
              id: element.id,
              name: element.equipmentChartName,
            });
          });
          this.isChartForEdit = true;
          this.currentForm = this.chartForms[0];
          const currId = this.currentForm.get('id')?.value;
          this.switchChartTab(currId);
        }
      }
    });
  }

  processChartTerms(
    chartForm: FormGroup,
    isType: boolean,
    terms: string,
    interestChart: InterestChart[],
    termsControlName: string,
    chartControlName: string,
    updateDateFlag: number
  ) {
    if (!terms) {
      chartForm.get(termsControlName)?.setValue([]);
      return;
    }

    if (isType && terms) {
      chartForm.get(termsControlName)?.setValue(terms.split(',').sort());
      this.populateFormArray(
        chartForm.get(chartControlName) as FormArray,
        interestChart,
        chartForm.get(termsControlName)?.value
      );
      interestChart.length > 0 &&
        this.setLastUpdatedDate(interestChart, updateDateFlag);
    }
  }

  populateFormArray(
    formArray: FormArray,
    chartData: InterestChart[],
    terms: string[]
  ) {
    while (formArray.length > 0) {
      formArray.removeAt(0);
    }

    const sortedData = chartData.sort((a, b) => {
      const aMinValue = parseFloat(a.minValue.replaceAll(',', String.Empty));
      const bMinValue = parseFloat(b.minValue.replaceAll(',', String.Empty));
      return aMinValue - bMinValue;
    });

    sortedData.forEach((data) => {
      const newFormGroup = this.createInterestChart();
      newFormGroup.get('id')?.setValue(data.id);
      newFormGroup
        .get('minValue')
        ?.setValue(this._decimalPipe.transform(data.minValue, '1.0-0'));
      newFormGroup
        .get('maxValue')
        ?.setValue(this._decimalPipe.transform(data.maxValue, '1.0-0'));
      newFormGroup.get('maxBrokerage')?.setValue(data.maxBrokerage);
      newFormGroup
        .get('defaultRateAdjustment')
        ?.setValue(data.defaultRateAdjustment);
      newFormGroup
        .get('maxBrokerageCeiling')
        ?.setValue(data.maxBrokerageCeiling);

      [24, 36, 48, 60, 72, 84].forEach((month) => {
        const key = `month${month}` as keyof InterestChart;
        if (terms.includes(`${month}`)) {
          newFormGroup.addControl(key, this._fb.control(data[key]));
        }
      });
      formArray.push(newFormGroup);
    });
  }

  setLastUpdatedDate(chartData: InterestChart[], type: number) {
    let lastUpdatedDate = new Date(chartData[0].updatedDate as string);
    chartData.forEach((item) => {
      const itemDate = new Date(item.updatedDate as string);
      if (lastUpdatedDate > itemDate) {
        lastUpdatedDate = itemDate;
      }
    });
    if (type == 1) {
      this.lastUpdateDateForChattel = lastUpdatedDate;
    } else {
      this.lastUpdateDateForRental = lastUpdatedDate;
    }
  }

  submitChartForms() {
    this.isFormSubmitted = true;
    let equipmentCharts: RateChartOption[] = [];
    this.validateInterestCharts();
    if (this.chartForms.some((formgroup) => formgroup.invalid)) {
      return;
    }

    this.chartForms.map((formGroup) => {
      const rawValue = formGroup.value;

      let rateChart = new RateChartOption(
        rawValue.id < 0 ? 0 : rawValue.id,
        rawValue.equipmentChartName,
        rawValue.typeOfFinance,
        rawValue.isInterestRatesVary,
        rawValue.chattelMortgageTerms,
        rawValue.rentalTerms,
        rawValue.selectedFunding,
        rawValue.interestChartChattelMortgage,
        rawValue.interestChartRental
      );

      let isChattel = rateChart.typeOfFinance.includes('Chattel mortgage');
      let isRental = rateChart.typeOfFinance.includes('Rental');

      rateChart.interestChartChattelMortgage.map((ele) => {
        ele.rateChartId = rateChart.id ?? 0;
        ele.minValue = ele.minValue?.toString().replaceAll(',', String.Empty);
        ele.maxValue = ele.maxValue?.toString().replaceAll(',', String.Empty);
        ele.typeOfFinance = 'Chattel mortgage';
      });
      rateChart.interestChartRental.map((ele) => {
        ele.maxValue = ele.maxValue ?? '0';
        ele.rateChartId = rateChart.id ?? 0;
        ele.minValue = ele.minValue?.toString().replaceAll(',', String.Empty);
        ele.maxValue = ele.maxValue?.toString().replaceAll(',', String.Empty);
        ele.typeOfFinance = 'Rental';
      });

      if (isChattel && isRental && !rateChart.isInterestRatesVary) {
        rateChart.chattelMortgageTerms = (
          rawValue.chattelMortgageTerms as string[]
        ).join(',');

        rateChart.rentalTerms = rateChart.chattelMortgageTerms;
        rateChart.interestChartRental = JSON.parse(
          JSON.stringify(rateChart.interestChartChattelMortgage)
        );
        rateChart.interestChartRental.map((ele) => {
          ele.typeOfFinance = 'Rental';
          ele.id = 0;
        });
      } else if (isChattel && isRental && rateChart.isInterestRatesVary) {
        rateChart.chattelMortgageTerms = (
          rawValue.chattelMortgageTerms as string[]
        ).join(',');
        rateChart.rentalTerms = isRental
          ? (rawValue.rentalTerms as string[]).join(',')
          : String.Empty;
      } else if (isChattel && !isRental) {
        (rateChart.rentalTerms = String.Empty),
          (rateChart.interestChartRental = []);
        rateChart.chattelMortgageTerms = (
          rawValue.chattelMortgageTerms as string[]
        ).join(',');
      } else if (isRental && !isChattel) {
        rateChart.rentalTerms = (
          rawValue.chattelMortgageTerms as string[]
        ).join(',');
        rateChart.interestChartRental = JSON.parse(
          JSON.stringify(rateChart.interestChartChattelMortgage)
        );
        rateChart.interestChartRental.map((ele) => {
          ele.typeOfFinance = 'Rental';
          ele.id = this.isOnlyRentalType ? ele.id : 0;
        });
        rateChart.chattelMortgageTerms = String.Empty;
        rateChart.interestChartChattelMortgage = [];
      }
      equipmentCharts.push(rateChart);
    });

    this._funderService
      .upsertFunderCharts(this.funderId, equipmentCharts)
      .subscribe(
        () => {
          this.getInterestChart();
          this._toaster.success(alertResponses.INTEREST_CHART_SAVED);
        },
        () => this._toaster.error(alertResponses.ERROR)
      );
  }

  onEditSubTabClick() {
    this.equipmentSubTabName = this.chartArray.find(
      (x) => x.id == this.currentTabIndex
    )?.name as string;
  }
  onSubTabNameSave() {
    this.chartArray.map((ele) => {
      if (ele.id == this.currentTabIndex) {
        ele.name = this.equipmentSubTabName;
      }
    });
    this.currentForm
      .get('equipmentChartName')
      ?.setValue(this.equipmentSubTabName);
    document.getElementById('subTabcancelbtn')?.click();
  }
  deleteTheSubTab() {
    this._funderService.deleteFunderChart(this.currentTabIndex).subscribe(
      () => {
        this.getInterestChart();
        this._toaster.success(alertResponses.INTEREST_CHART_DELETE);
      },
      () => this._toaster.error(alertResponses.ERROR)
    );
    document.getElementById('deletSubtabCancel')?.click();
  }

  validateInterestCharts() {
    this.chartForms.forEach((formGroup) => {
      const formValue = formGroup.value;
      const interestChartChattelMortgageArray = formGroup.get(
        FunderChartConstants.INTEREST_CHART_CHATTEL_MORTGAGE
      ) as FormArray;
      const interestChartRentalArray = formGroup.get(
        FunderChartConstants.INTEREST_CHART_RENTAL
      ) as FormArray;

      let isChattel = formValue.typeOfFinance.includes('Chattel mortgage');
      let isRental = formValue.typeOfFinance.includes('Rental');
      let isRatesVary = formValue.isInterestRatesVary;

      this.validateArrayField(
        formGroup,
        'selectedFunding',
        formValue.selectedFunding.length
      );

      if (isChattel && isRental && !isRatesVary) {
        this.validateArrayField(
          formGroup,
          FunderChartConstants.CHATTEL_MORTGAGE_TERMS,
          formValue.chattelMortgageTerms.length
        );
        this.validateChartArray(
          interestChartChattelMortgageArray,
          formGroup,
          1
        );
      } else if (isChattel && isRental && isRatesVary) {
        this.validateArrayField(
          formGroup,
          FunderChartConstants.CHATTEL_MORTGAGE_TERMS,
          formValue.chattelMortgageTerms.length
        );
        this.validateChartArray(
          interestChartChattelMortgageArray,
          formGroup,
          1
        );
        this.validateArrayField(
          formGroup,
          FunderChartConstants.RENTAL_TERMS,
          formValue.rentalTerms.length
        );
        this.validateChartArray(interestChartRentalArray, formGroup, 2);
      } else if (isChattel && !isRental) {
        this.validateArrayField(
          formGroup,
          FunderChartConstants.CHATTEL_MORTGAGE_TERMS,
          formValue.chattelMortgageTerms.length
        );
        this.validateChartArray(
          interestChartChattelMortgageArray,
          formGroup,
          1
        );
      } else if (isRental && !isChattel) {
        this.validateArrayField(
          formGroup,
          FunderChartConstants.CHATTEL_MORTGAGE_TERMS,
          formValue.chattelMortgageTerms.length
        );
        this.validateChartArray(
          interestChartChattelMortgageArray,
          formGroup,
          1
        );
      }

      if (formGroup.invalid) {
        let errorMsg = alertResponses.ON_FORM_INVALID;
        const invalidFields = Object.keys(formGroup.controls)
          .filter((field) => formGroup.get(field)?.invalid)
          .map((field) => `<br> - ${field}`);
        this._toaster.error(
          `${errorMsg} ${invalidFields} <br> in ${formValue.equipmentChartName}`,
          String.Empty,
          {
            enableHtml: true,
          }
        );
        return;
      }
    });
  }
  validateArrayField(formGroup: FormGroup, fieldName: string, length: number) {
    const field = formGroup.get(fieldName);
    if (length === 0) {
      field?.setErrors({ arrayIsEmpty: true });
    } else {
      field?.setErrors(null);
    }
  }

  validateChartArray(
    chartArray: FormArray,
    formGroup: FormGroup,
    type: number
  ) {
    if (chartArray.controls.length > 0) {
      chartArray.controls.forEach((chartGroup, index) => {
        this.validateMinValueField(chartGroup, formGroup, index, type);
        this.validateMaxValueField(chartGroup, index, type);
      });

      this.validateArrayHasMinValue(chartArray);
    } else {
      chartArray.setErrors({ arrayIsEmpty: true });
    }
  }
  validateArrayHasMinValue(chartArray: FormArray) {
    if (chartArray.controls.length <= 1) {
      const firstFormGroup = chartArray.controls.at(0) as FormGroup;
      const minValue = firstFormGroup?.get('minValue')?.value;
      if (!minValue) {
        chartArray.setErrors({ arrayIsEmpty: true });
      } else {
        chartArray.setErrors(null);
      }
    } else {
      chartArray.setErrors(null);
    }
  }

  formChangeHandler() {
    this.isFunderChartFormDirty.emit(true);
  }
}
