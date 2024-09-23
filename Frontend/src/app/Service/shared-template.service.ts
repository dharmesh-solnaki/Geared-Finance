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
  private templateRef!: TemplateRef<HTMLElement> | null;
  private headerTemplateRef!: TemplateRef<HTMLElement> | null;

  setTemplate(template: TemplateRef<HTMLElement> | null) {
    this.templateRef = template;
  }
  getTemplate(): TemplateRef<HTMLElement> | null {
    return this.templateRef;
  }

  setHeaderTemplate(template: TemplateRef<HTMLElement> | null) {
    this.headerTemplateRef = template;
  }
  getHeaderTemplate(): TemplateRef<HTMLElement> | null {
    return this.headerTemplateRef;
  }
}
