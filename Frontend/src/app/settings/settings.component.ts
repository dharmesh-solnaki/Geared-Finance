import { Component, OnInit } from '@angular/core';
import {
  SettingMenuType,
  settingSystemProperties,
  settingsSalesAndMarketing,
} from '../Shared/constants';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class SettingsComponent implements OnInit {
  systemPropertiesData: SettingMenuType[] = [];
  salesAndMarketingData: string[] = [];
  isSdiebarVisible: boolean = false;
  ngOnInit(): void {
    this.systemPropertiesData = settingSystemProperties;
    this.salesAndMarketingData = settingsSalesAndMarketing;
  }

  toggleSidebar(){
    this.isSdiebarVisible = !this.isSdiebarVisible;
  }
}
