import { Injectable, TemplateRef } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { HeaderSearchModel } from '../Models/common-models';

@Injectable({
  providedIn: 'root',
})
export class SharedTemplateService {
  constructor() {}

  isSearchRequired: boolean = false;
  searchSubject = new Subject<string>();
  searchList = new BehaviorSubject<HeaderSearchModel[]>([]);
  searchedId = new BehaviorSubject<number>(0);
  isSearchCleared = new Subject<boolean>();
  private templateRef!: TemplateRef<any> | null;
  private headerTemplateRef!: TemplateRef<any> | null;

  setTemplate(template: TemplateRef<any> | null) {
    this.templateRef = template;
  }
  getTemplate(): TemplateRef<any> | null {
    return this.templateRef;
  }

  setHeaderTemplate(template: TemplateRef<any> | null) {
    this.headerTemplateRef = template;
  }
  getHeaderTemplate(): TemplateRef<any> | null {
    return this.headerTemplateRef;
  }
}
