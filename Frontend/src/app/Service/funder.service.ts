import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import {
  FunderFormType,
  FunderGuideType,
} from '../Models/funderFormType.model';
import { Observable, ObservedValueOf } from 'rxjs';
import { CommonSearch } from '../Models/common-search.model';
import { BaseResponse } from '../Models/common-models';
import { Funder } from '../Models/funder.model';

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
 
  getFunderGuide(id:number):Observable<FunderGuideType>{
    return this._http.get<FunderGuideType>(`${this.API_URL}/FunderGuide?id=${id}`)
  }

  upsertFunder(funder: FunderFormType): Observable<number> {
    return this._http.post<number>(`${this.API_URL}/Funder`, funder);
  }
  upsertFunderGuide(funderGuide: FunderGuideType) :Observable<number> {
    return this._http.post<number>(`${this.API_URL}/FundigGuide`, funderGuide);
  }
}
