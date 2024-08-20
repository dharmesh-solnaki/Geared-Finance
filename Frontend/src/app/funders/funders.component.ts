import { Component, TemplateRef, ViewChild } from '@angular/core';
import { SharedTemplateService } from '../Models/shared-template.service';

@Component({
  selector: 'app-funders',
  templateUrl: './funders.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class FundersComponent {
  @ViewChild('funderDefaultHeader', { static: true })
  funderDefaultHeader!: TemplateRef<any>;

  constructor(private _templateService: SharedTemplateService) {
    console.log('hi fmrom ctor4');
  }
  ngOnInit(): void {
    console.log('hi from slkdj');
    this._templateService.setTemplate(this.funderDefaultHeader);
  }
  ngOnDestroy(): void {
    this._templateService.setTemplate(null);
  }
}
