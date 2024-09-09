import { CommonTransfer } from './common-transfer.model';

export class FunderFormType {
  constructor(
    public name: string,
    public abn: string,
    public status: boolean,
    public bank: string,
    public bsb: string,
    public account: string,
    public streetAddress: string,
    public suburb: string,
    public state: string,
    public postcode: string,
    public postalAddress: string,
    public postalSuburb: string,
    public postalState: string,
    public postalPostcode: string,
    public creditAppEmail: string,
    public settlementsEmail: string,
    public adminEmail: string,
    public payoutsEmail: string,
    public collectionEmail: string,
    public eotEmail: string,
    public bdmName: string,
    public bdmSurname: string,
    public bdmEmail: string,
    public bdmPhone: string,
    public id: number = 0
  ) {}
}

export class FunderGuideType {
  constructor(
    public funderId: number = 0,
    public financeType: string,
    public rates: string,
    public isBrokerageCapped: boolean = false,
    public selectedFundings: CommonTransfer[],
    public isApplyRITCFee: boolean = false,
    public ritcFee?: string,
    public isAccountKeepingFee: boolean = false,
    public accountKeepingFee?: string,
    public isApplyDocumentFee: boolean = false,
    public funderDocFee?: string,
    public matrixNotes?: string,
    public generalNotes?: string,
    public eotNotes?: string,
    public cutoff?: string,
    public craa?: string
  ) {}
}
