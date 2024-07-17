import { Component, OnInit } from '@angular/core';
import {
  settingSystemProperties,
  settingsSalesAndMarketing,
} from '../Models/constants.model';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class SettingsComponent implements OnInit {
  systemPropertiesData: string[] = [];
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
