import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-funder-chart',
  templateUrl: './funder-chart.component.html',
  styleUrls: ['../../../assets/Styles/appStyle.css'],
})
export class FunderChartComponent {
  chartArray = ['equipment 1'];
  currentForm!: FormGroup;
  constructor(private _fb: FormBuilder) {}

  ngOnInit(): void {
    this.initializeCurrentForm();
  }

  initializeCurrentForm() {
    this.currentForm = this._fb.group({
      id: [],
    });
  }

  addNewChart() {
    this.chartArray = [
      ...this.chartArray,
      `equipment ${this.chartArray.length + 1}`,
    ];
  }
}
