import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SharedTemplateService } from 'src/app/Models/shared-template.service';
import {
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
  selectMenuStatus: selectMenu[] = [];
  funderForm: FormGroup = new FormGroup({});

  @ViewChild('overViewTemplate', { static: true })
  overViewTemplate!: TemplateRef<any>;
  @ViewChild('funderProductTypeGuide', { static: true })
  funderProductTypeGuide!: TemplateRef<any>;
  @ViewChild('funderHeaderTabs', { static: true })
  funderHeaderTabs!: TemplateRef<any>;
  constructor(
    private _templateService: SharedTemplateService,
    private _fb: FormBuilder
  ) {}

  ngOnInit() {
    this.setActiveTab('overview', this.overViewTemplate);
    console.log(this.funderHeaderTabs);
    this._templateService.setTemplate(this.funderHeaderTabs);
    this.selectMenuStatus = statusSelectMenu;
    this.initializeFunderForm();
  }

  initializeFunderForm() {
    this.funderForm = this._fb.group({
      name: ['', [Validators.required]],
      abn: [
        '',
        [
          Validators.required,
          Validators.minLength(11),
          Validators.maxLength(11),
        ],
      ],
      status: [, Validators.required],
      bank: [''],
      bsb: ['', [Validators.minLength(6), Validators.maxLength(6)]],
      account: [''],
      streetAddress: [''],
      suburb: [''],
      state: [''],
      postcode: ['', [Validators.minLength(4)]],
      postalAddress: [''],
      postalSuburb: [''],
      postalState: [''],
      postalPostcode: ['', [Validators.minLength(4)]],
      creditAppEmail: ['', [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      settlementsEmail: [
        '',
        [Validators.pattern(validationRegexes.EMAIL_REGEX)],
      ],
      adminEmail: ['', [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      payoutsEmail: ['', [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      collectionEmail: [
        '',
        [Validators.pattern(validationRegexes.EMAIL_REGEX)],
      ],
      eotEmail: ['', [Validators.pattern(validationRegexes.EMAIL_REGEX)]],
      bdmName: ['', [Validators.required]],
      bdmSurname: ['', [Validators.required]],
      bdmEmail: [
        '',
        [
          Validators.required,
          Validators.pattern(validationRegexes.EMAIL_REGEX),
        ],
      ],
      bdmPhone: [''],
    });
  }

  setActiveTab(tab: string, template: TemplateRef<any>) {
    this.activeTab = tab;
    this.activeTemplate = template;
  }
}
