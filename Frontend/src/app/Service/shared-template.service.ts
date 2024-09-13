import { Injectable, TemplateRef } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SharedTemplateService {
  constructor() {}

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
