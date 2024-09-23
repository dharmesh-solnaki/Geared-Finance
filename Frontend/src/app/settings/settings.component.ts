import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import {
  SettingMenuType,
  settingSystemProperties,
  settingsSalesAndMarketing,
} from '../Shared/constants';
import { SharedTemplateService } from '../Service/shared-template.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class SettingsComponent implements OnInit {
  systemPropertiesData: SettingMenuType[] = [];
  salesAndMarketingData: string[] = [];
  isSdiebarVisible: boolean = false;
  @ViewChild('SettignsHeader', { static: true })
  settignsHeader!: TemplateRef<HTMLElement>;
  constructor(private _templateService: SharedTemplateService) {}

  ngOnInit(): void {
    this.systemPropertiesData = settingSystemProperties;
    this.salesAndMarketingData = settingsSalesAndMarketing;
    this._templateService.setHeaderTemplate(this.settignsHeader);
  }
  ngOnDestroy(): void {
    this._templateService.setHeaderTemplate(null);
  }

  toggleSidebar() {
    this.isSdiebarVisible = !this.isSdiebarVisible;
  }
}
