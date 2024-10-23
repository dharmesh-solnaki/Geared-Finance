import { CommonTransfer } from './common-transfer.model';

export class RateChartOption {
  constructor(
    public id: number,
    public equipmentChartName: string,
    public typeOfFinance: string,
    public isInterestRatesVary: boolean,
    public chattelMortgageTerms: string,
    public rentalTerms: string,
    public selectedFunding: CommonTransfer[],
    public interestChartChattelMortgage: InterestChart[],
    public interestChartRental: InterestChart[]
  ) {}
}

export class InterestChart {
  constructor(
    public id: number,
    public typeOfFinance: string,
    public minValue: string,
    public maxValue: string,
    public rateChartId: number,
    public defaultRateAdjustment: number,
    public month24?: number,
    public updatedDate?: string,
    public month36?: number,
    public month48?: number,
    public month60?: number,
    public month72?: number,
    public month84?: number,
    public maxBrokerage?: number,
    public maxBrokerageCeiling?: number
  ) {}
}

export class RateChartResponse {
  constructor(
    public availableFundings: CommonTransfer[],
    public rateCharts: RateChartOption[],
    public typeOfFinance: string
  ) {}
}
