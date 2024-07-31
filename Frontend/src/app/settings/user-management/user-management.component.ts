import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
  
} from 'src/app/Models/common-grid.model';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';
import { roleSelectionMenu } from 'src/app/Shared/constants';
import { UserService } from '../../Service/user.service';
import { User, UserGridSetting } from '../../Models/user.model';
import { CommonSearch } from 'src/app/Models/common-search.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  // styleUrls:['../../../assets/Styles/appStyle.css']
})
export class UserManagementComponent implements OnInit {
  selectMenuRoles: { option: string; value: string }[] = [];
  userData: User[] = [];
  totalRecords:number=0
  gridSetting!: IGridSettings;
  paginationSettings!: PaginationSetting;
  userHeaderSearchForm: FormGroup;

  searchingModel:CommonSearch =  {
    pageNumber:1,
    pageSize:10,    
  }

  @ViewChild('roleSelectionMenu') roleSelectionMenu!: CommonSelectmenuComponent;

  constructor(private _fb: FormBuilder,private _userService:UserService, private _router:Router,private _route: ActivatedRoute) {
    this.userHeaderSearchForm = this._fb.group({
      searchString: [''],
      selectedRole: [''],
    });    
  }

  ngOnInit() {
    this.selectMenuRoles = roleSelectionMenu; 
    this.gridSetting = UserGridSetting;
    this.userDataSetter();
  }
  

  userDataSetter(){
   this.userData=[];
    this._userService.getUsers(this.searchingModel).subscribe(res=>{
   
     if(  res && res.responseData) {
      this.totalRecords = res.totalRecords
      this.userData= res.responseData
      this.userData.map(e=>{e.venodrName=e.vendor?.name})
     }  
     this.paginationSetter();
      },
      err=>{
        console.log(err)
      })
   
    }

  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.totalRecords,
      currentPage: this.searchingModel.pageNumber,
      // selectedPageSize: ['25 per page', '50 per page', '100 per page'],
      selectedPageSize: [`${this.searchingModel.pageSize} per page`],
    };
  }

  pageChangeEventHandler(page:number){
      this.searchingModel.pageNumber=page;
      this.userDataSetter()
  }
  pageSizeChangeHandler(pageSize:number){
  this.searchingModel.pageNumber=1;
  this.searchingModel.pageSize=pageSize
  this.userDataSetter()
  }

  onEditEventRecevier(id:number){   
    this._router.navigate([`${id}/Edit`], {relativeTo:this._route ,state:this.userData.filter(e=>e.id==id)})
  }
  
  searchHandler() {
    this.searchingModel = {
      name: this.userHeaderSearchForm.get('searchString')?.value|| '',
      pageNumber:1,
      pageSize:10, 
      roleName:  this.userHeaderSearchForm.get('selectedRole')?.value
    }
    this.userDataSetter();
  }

  resetForm() {
    const keyword = this.userHeaderSearchForm.get('searchString')?.value
    const role = this.userHeaderSearchForm.get('selectedRole')?.value
   if(keyword ||role){
    this.userHeaderSearchForm.reset();
    this.roleSelectionMenu.resetElement();
    this.paginationSetter();
    this.searchingModel={
      pageSize:10,
      pageNumber:1
    }
    this.userDataSetter()
   }

  }

  sortHandler(ev: SortConfiguration) {
    const { sort , sortOrder } = ev;
    this.searchingModel.sortBy=sort.trim();
    this.searchingModel.sortOrder=sortOrder
    this.userDataSetter();
  }
}
