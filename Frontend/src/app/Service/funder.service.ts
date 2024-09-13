import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import {
  FunderFormType,
  FunderGuideType,
} from '../Models/funderFormType.model';
import { Observable } from 'rxjs';
import { CommonSearch } from '../Models/common-search.model';
import { BaseResponse } from '../Models/common-models';
import { Funder } from '../Models/funder.model';
import { Document } from '../Models/document.model';
@Injectable({
  providedIn: 'root',
})
export class FunderService {
  private API_URL = `${environment.BASE_URL}/Funder`;
  constructor(private _http: HttpClient) {}

  getFunders(searchModel: CommonSearch): Observable<BaseResponse<Funder>> {
    return this._http.post<BaseResponse<Funder>>(
      `${this.API_URL}/Funders`,
      searchModel
    );
  }
  getFunder(id: number): Observable<FunderFormType> {
    return this._http.get<FunderFormType>(`${this.API_URL}?id=${id}`);
  }

  getFunderGuide(id: number): Observable<FunderGuideType> {
    return this._http.get<FunderGuideType>(
      `${this.API_URL}/FunderGuide?id=${id}`
    );
  }
  getDocList(
    searchingModel: CommonSearch,
    id: number
  ): Observable<BaseResponse<Document>> {
    return this._http.post<BaseResponse<Document>>(
      `${this.API_URL}/Documents?id=${id}`,
      searchingModel
    );
  }
  getDocument(fileName: string): Observable<Blob> {
    return this._http.get(`${this.API_URL}/Document?docName=${fileName}`, {
      responseType: 'blob',
    });
  }

  upsertFunder(funder: FunderFormType): Observable<number> {
    return this._http.post<number>(`${this.API_URL}/Funder`, funder);
  }
  upsertFunderGuide(funderGuide: FunderGuideType): Observable<number> {
    return this._http.post<number>(`${this.API_URL}/FundingGuide`, funderGuide);
  }
  uploadImg(formData: FormData, funderId: number) {
    return this._http.post(
      `${this.API_URL}/LogoImage?id=${funderId}`,
      formData
    );
  }
  uploadDocument(formData: FormData, funderId: number) {
    return this._http.post(`${this.API_URL}/Document?id=${funderId}`, formData);
  }

  deleteDocument(id: number) {
    return this._http.delete(`${this.API_URL}/Document?id=${id}`);
  }
  deleteFunder(id: number) {
    return this._http.delete(`${this.API_URL}/Funder/${id}`);
  }
}
