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

  constructor(private _fb: FormBuilder,private _userService:UserService) {
    this.userHeaderSearchForm = this._fb.group({
      searchString: [''],
      selectedRole: [''],
    });    
  }

  ngOnInit() {
    this.selectMenuRoles = roleSelectionMenu; 
    this.gridSetting = UserGridSetting;
    this.dataSetter();
    this.paginationSetter();
  }

  dataSetter(){
   this.userData=[];
    this._userService.getUsers(this.searchingModel).subscribe(res=>{

      this.userData=res
    this.userData.map(x => {
       x.staffcode = x.name.charAt(0).concat(x.surName.slice(0,2)).toUpperCase()
    });
      })
      }

  paginationSetter() {
    this.paginationSettings = {
      totalRecords: this.userData.length,
      currentPage: 1,
      selectedPageSize: ['25 pre page', '50 per page', '100 per page'],
    };
  }

  
  searchHandler() {
    this.searchingModel = {
      name: this.userHeaderSearchForm.get('searchString')?.value|| '',
      pageNumber:1,
      pageSize:10, 
      roleName:  this.userHeaderSearchForm.get('selectedRole')?.value
    }

    this.dataSetter();
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
    this.dataSetter()
  }

  sortHandler(ev: SortConfiguration) {
    const { sort , sortOrder } = ev;
  
    this.searchingModel.sortBy=sort.trim();
    this.searchingModel.sortOrder=sortOrder
    this.dataSetter();
  }
}
