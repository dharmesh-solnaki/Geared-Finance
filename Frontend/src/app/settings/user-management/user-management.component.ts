import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  IGridSettings,
  PaginationSetting,
  SortConfiguration,
  
} from 'src/app/Models/common-grid.model';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';
import { roleSelectionMenu } from 'src/app/Models/constants.model';
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
    this.paginationSetter();
  }

  userDataSetter(){
   this.userData=[];
    this._userService.getUsers(this.searchingModel).subscribe(res=>{
   
      this.userData=res;
      this.userData.map(e=>{e.venodrName=e.vendor?.name})    
      },
      err=>{
        console.log(err)
      })}

  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.userData.length,
      currentPage: 1,
      selectedPageSize: ['25 pre page', '50 per page', '100 per page'],
    };
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
    this.paginationSetter();
  }

  resetForm() {
    this.userHeaderSearchForm.reset();
    this.roleSelectionMenu.resetElement();
    this.paginationSetter();
    this.searchingModel={
      pageSize:10,
      pageNumber:1
    }
    this.userDataSetter()
  }

  sortHandler(ev: SortConfiguration) {
    const { sort , sortOrder } = ev;
  
    this.searchingModel.sortBy=sort.trim();
    this.searchingModel.sortOrder=sortOrder
    this.userDataSetter();
  }
}
