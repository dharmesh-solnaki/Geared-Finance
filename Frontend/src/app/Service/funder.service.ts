import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import {
  FunderFormType,
  FunderGuideType,
} from '../Models/funderFormType.model';
import { Observable } from 'rxjs';
import { CommonSearch } from '../Models/common-search.model';
import { BaseResponse, HeaderSearchModel } from '../Models/common-models';
import { Funder } from '../Models/funder.model';
import { Document } from '../Models/document.model';
import { Note } from '../Models/note.model';
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
  getFunderSearch(search: string): Observable<HeaderSearchModel[]> {
    return this._http.get<HeaderSearchModel[]>(
      `${this.API_URL}/Funder?keyword=${search}`
    );
  }
  getNotes(
    searchModel: CommonSearch,
    funderId: number
  ): Observable<BaseResponse<Note>> {
    return this._http.post<BaseResponse<Note>>(
      `${this.API_URL}/Notes/${funderId}`,
      searchModel
    );
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
  upsertNote(funderId: number, note: Note): Observable<number> {
    return this._http.post<number>(`${this.API_URL}/Note/${funderId}`, note);
  }

  deleteDocument(id: number) {
    return this._http.delete(`${this.API_URL}/Document?id=${id}`);
  }
  deleteFunder(id: number) {
    return this._http.delete(`${this.API_URL}/Funder/${id}`);
  }
  deleteNote(id: number) {
    return this._http.delete(`${this.API_URL}/Note/${id}`);
  }
}
