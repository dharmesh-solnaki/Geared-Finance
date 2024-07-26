import {   Component, OnInit, } from '@angular/core';
import { FormBuilder, FormGroup,  Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {  forkJoin, of, } from 'rxjs';
import { CommonSearch } from 'src/app/Models/common-search.model';
import {PhonePipe} from 'src/app/Pipes/phone.pipe'
import {
  dateSelectonMenu,
  monthSelectonMenu,
  notificationPreSelectionMenu,
  roleSelectionMenu,
  selectMenu,
  RoleEnum,
  validationRegexes,
  MonthEnum,
  alertResponses,
} from 'src/app/Models/constants.model';
import { User } from 'src/app/Models/user.model';
import { UserService } from 'src/app/Service/user.service';
import { VendorService } from 'src/app/Service/vendor.service';



@Component({
  selector: 'app-add-site-user',
  templateUrl: './add-site-user.component.html',
})
export class AddSiteUserComponent implements OnInit {
  selectMenuRoles: selectMenu[] = [];
  selectMenuVendors: selectMenu[] = [];
  selectMenuNotificationPref: selectMenu[] = [];
  selectMenuMonth: selectMenu[] = [];
  selectMenuDate: selectMenu[] = [];
  defaultDateSelection: selectMenu[] = [];
  defaultMonthSelection: selectMenu[] = [];
  selectMenuRelationshipManager: selectMenu[] = [];
  selectMenuReportingTo:selectMenu[]=[]
  selectMenuManagerLevels:selectMenu[]=[]
  userForm: FormGroup = new FormGroup({});
  isEditable: boolean = false;
  isShowFieldReportTo:boolean=true;
  isFormSubmitted:boolean=false;
  userDataOnEdit: User = new User(0, '', '', '', '');

  constructor(
    private _fb: FormBuilder,
    private _userService: UserService,
    private _route: ActivatedRoute,
    private _router:Router,
    private _vendorService:VendorService
  ) {
    this.formInitializer();

  }

  ngOnInit(): void {
    this.selectMenuRoles = roleSelectionMenu;
    this.selectMenuNotificationPref = notificationPreSelectionMenu;
    this.defaultDateSelection = dateSelectonMenu(31);
    this.defaultMonthSelection = monthSelectonMenu(-1);
    this.selectMenuMonth = this.defaultMonthSelection;
    this.selectMenuDate = this.defaultDateSelection;
    this.addFormBirthHandler();

     
    this.isEditable = this._route.snapshot.params['id'] != undefined;
    if (this.isEditable) {
      const id = this._route.snapshot.params['id'];
      if (id) {
        const userSearch: CommonSearch = {
          pageNumber: 1,
          pageSize: 1,
          id: id,
        };

        this._userService.getUsers(userSearch).subscribe((res) => {
          this.userDataOnEdit = res[0];
          this.userFormInitializerOnEdit();
        });
      }
    }

    this.userForm.controls['role'].valueChanges.subscribe((res) => {
      this.userForm.markAsUntouched();
      const role=res
      if (
        [ RoleEnum.VendorGuestUser,
       RoleEnum.VendorManager,
       RoleEnum.VendorSalesRep].includes(res)
      ) {
      
         this.vendorDataSetter()
        this.relationshipManagerDataSetter();

        
          this.userForm.controls['vendor'].valueChanges.subscribe(res=>{ 
            const vendorId=res||0; 
          
            this.reportingToDataSetter(vendorId,0)  
            if(role ==  RoleEnum.VendorManager){
            this.managerLevelsDataSetter(res)
           
            this.userForm.controls['vendorManagerLevel'].valueChanges.subscribe(res=>{
              const managerLevelId = res||0
              let maxLevel  = 0;
              let selectedValueLevel=0;
              this.selectMenuManagerLevels.forEach(e=> {
                 let tempMax =   +e.option.charAt(e.option.length-1)
             
                 if(tempMax >= maxLevel){
                   maxLevel=tempMax
                 }
                 if(managerLevelId==e.value){
                  selectedValueLevel=tempMax
                 }
              })
             if(maxLevel==selectedValueLevel){
                this.isShowFieldReportTo=false;
             }else{
              this.isShowFieldReportTo=true;
              this.reportingToDataSetter(vendorId,managerLevelId);
             }
            })
            }
          })
        }
    });
   
  }
  userFormInitializerOnEdit() {
    const formdata = this.userDataOnEdit;
    const searchModel:CommonSearch ={
      pageNumber: 1,
      pageSize: Number.INT_MAX_VALUE
    } 
    formdata.vendorManagerLevelId = formdata.vendorManagerLevelId||0
     forkJoin(
      [         
        this._vendorService.getVendors(searchModel),
        this._userService.getRelationshipManagers(),
        formdata.vendor ? this._vendorService.getManagerLevels(formdata.vendor.id):of([]),
        formdata.vendorId ?  this._userService.getReportingTo(formdata.vendorId,formdata.vendorManagerLevelId):of([])
  
    ]).subscribe((res)=>{   
        res[0].map(e=> {
          this.selectMenuVendors.push({ option: e.name, value: e.id })
       });
        res[1].map(e=> {
          this.selectMenuRelationshipManager.push({ option: `${e.name} ${e.surName}${e.status ? '' : ' - inactive'}`, value: e.id });
       });
       res[2] && res[2].map(e=> {
          this.selectMenuManagerLevels.push({  option:`${e.levelName} - Level ${e.levelNo}`, value: e.id });
       });
       res[3] && res[3].map(e=> {
          this.selectMenuReportingTo.push({ option:`${e.name} ${e.surName}`, value: e.id });
       });
      
       const pipe =new PhonePipe(); 
       this.userForm.patchValue({
      name: formdata.name,
      surname: formdata.surName,
      email: formdata.email,
      password: formdata.password,
      role: formdata.roleName,
      notificationPref: formdata.notificationPreferences,
      month: formdata.monthOfBirth,
      day: formdata.dayOfBirth,
      mobileNumber: pipe.transform(formdata.mobile),
      roleStatus: formdata.status,
      portalLogin: formdata.isPortalLogin,
      sendEOTreports: formdata.isSendEndOfTermReport,
      vendorSalesRepList: formdata.isUserInVendorSalesRepList,
      unassignedApplications: formdata.unassignedApplications,
      funderProfile: formdata.isFunderProfile,
      proceedBtnInApp: formdata.isProceedBtnInApp,
      clacRateEditor: formdata.isCalcRateEditor,
      gearedSalesRepList: formdata.isUserInGafsalesRepList,
      reportTo: formdata.reportingTo,
      relationshipManager: formdata.relationshipManager,
      vendor:formdata.vendor && +formdata.vendor.id,
      vendorManagerLevel: formdata.vendorManagerLevelId
    });
  })
  }
  

  addFormBirthHandler() {
    this.userForm.get('month')?.valueChanges.subscribe((selectedMonth) => {
      const daysInMonth = this.getDaysInMonth(selectedMonth);
      this.selectMenuDate = this.defaultDateSelection.filter(
        (x) => Number(x.value) <= daysInMonth
      );
    });

    this.userForm.get('day')?.valueChanges.subscribe((selectedDay) => {
      this.updateMonthSelection(selectedDay);
    });
  }

  formInitializer() {
    this.userForm = this._fb.group({
      name: ['', [Validators.required]],
      surname: ['', [Validators.required]],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(validationRegexes.EMAIL_REGEX),
        ],
      ],
      password: ['', [Validators.pattern(validationRegexes.PASSWORD_REGEX)]],
      role: ['', Validators.required],
      vendor: [],
      notificationPref: [0, [Validators.required]],
      month: [0],
      day: [0],
      mobileNumber: ['', [Validators.required]],
      roleStatus: [true],
      portalLogin: [true],
      sendEOTreports: [true],
      vendorSalesRepList: [true],
      unassignedApplications: [true],
      funderProfile: [true],
      proceedBtnInApp: [false],
      clacRateEditor: [false],
      gearedSalesRepList: [false],
      vendorManagerLevel: [],
      reportTo: [],
      relationshipManager: [],
    });
  }

  getDaysInMonth(month: number): number {
    switch (month) {
      case MonthEnum.Feb:
        return this.isLeapYear(new Date().getFullYear()) ? 29 : 28;
      case MonthEnum.Apr:
      case MonthEnum.Jun:
      case MonthEnum.Sep:
      case MonthEnum.Nov:
        return 30;
      default:
        return 31;
    }
  }

  isLeapYear(year: number): boolean {
    return (year % 4 === 0 && year % 100 !== 0) || year % 400 === 0;
  }

  updateDateSelection(daysInMonth: number) {
    if (daysInMonth === -1) {
      this.selectMenuDate = this.defaultDateSelection;
    } else {
      this.selectMenuDate = this.selectMenuDate.filter(
        (x) => +x.value <= daysInMonth
      );
    }
  }

  updateMonthSelection(selectedDay: number) {
    if (selectedDay > 0) {
      const validMonths = this.defaultMonthSelection.filter((month) => {
        const daysInMonth = this.getDaysInMonth(+month.value);
        return selectedDay <= daysInMonth;
      });
      this.selectMenuMonth = validMonths;
    } else {
      this.selectMenuMonth = this.defaultMonthSelection;
    }
  }

  isFieldVisible(field: string) {
    const role = this.userForm.get('role')?.value;

    switch (field) {
      case 'password':
        return (
          (role == RoleEnum.GearedAdmin ||
            role == RoleEnum.GearedSuperAdmin ||
            role == RoleEnum.GearedSalesRep) &&
          this.userForm.get('portalLogin')?.value
        );
      case 'vendor':
        return (
          role == RoleEnum.VendorSalesRep ||
          role == RoleEnum.VendorManager ||
          role == RoleEnum.VendorGuestUser
        );
      case 'reportTo':
        return (
          (role == RoleEnum.VendorGuestUser ||
            role == RoleEnum.VendorManager ||
            role == RoleEnum.VendorSalesRep) && this.isShowFieldReportTo
        );

      case 'portalLogin':
        return role != RoleEnum.VendorGuestUser;

      case 'vendorManagerLevel':
        return role == RoleEnum.VendorManager;

      case 'relaionshipManager':
        return (
          role == RoleEnum.VendorSalesRep ||
          role == RoleEnum.VendorGuestUser ||
          role == RoleEnum.VendorManager
        );
      case 'birthDate':
        return (
          role == RoleEnum.VendorGuestUser ||
          role == RoleEnum.VendorManager ||
          role == RoleEnum.VendorSalesRep
        );

      //to show main fields
      case 'reports':
        return role == RoleEnum.GearedSalesRep;

      case 'roleOptions':
        return (
          role == RoleEnum.GearedSuperAdmin ||
          role == RoleEnum.GearedSalesRep ||
          role == RoleEnum.VendorManager
        );
      case 'roleOptionsGearedSalesRep':
        return role == RoleEnum.GearedSalesRep;

      case 'roleOptionsGearedVendorManager':
        return role == RoleEnum.VendorManager;

      case 'roleOptionsGearedSuperAdmin':
        return role == RoleEnum.GearedSuperAdmin;

      default:
        return true;
    }
  }

  userFormHandler() {
    this.isFieldRequired();
    this.isFormSubmitted = true
    console.log(this.userForm.invalid)
    console.log(this.userForm)
    if (this.userForm.invalid) {    
      this.userForm.markAllAsTouched();
      return;
    }
    const formValues = this.userForm.value;      
    
        const user = new User(
          this.isEditable ? this.userDataOnEdit.id : 0,
          formValues.name,
          formValues.surname,
          formValues.email,
          formValues.mobileNumber.replace(' ', '').replace(' ', '').slice(0, 10),
          formValues.password,
          formValues.notificationPref,
          formValues.roleStatus,
          formValues.portalLogin,
          formValues.gearedSalesRepList,
          formValues.day,
          formValues.month,
          formValues.relationshipManager,
          formValues.reportTo,
          formValues.vendorSalesRepList,
          formValues.unassignedApplications,
          formValues.role,
          formValues.sendEOTreports,
          formValues.funderProfile,
          formValues.proceedBtnInApp,
          formValues.clacRateEditor,
          this.isEditable ? this.userDataOnEdit.staffCode : '',
          formValues.vendor==0?null:formValues.vendor,
          formValues.vendorManagerLevel && formValues.vendorManagerLevel.toString()
        );
    
        // if (this.isEditable) {
        //   console.log(user)
        //       this._userService.updateUser(user).subscribe(
        //       (res) => {
        //         alert(alertResponses.UPDATE_RECORD);
        //       },
        //       (err) => {
        //         console.log(err);
        //       }
        //     );
        // } else {
   
          this._userService.addUser(user).subscribe(
            (res) => {
              if(res.isEmailExist || res.isExistMobile){
                alert( `${res.isEmailExist?'email':''} ${res.isExistMobile?'mobile':''} already exist`)
              }else {
                alert(this.isEditable?alertResponses.UPDATE_RECORD:alertResponses.ADD_RECORD);
                this._router.navigate(['settings/user-management'])
              }
            },
            (err) => {
              console.log(err);
            }
          );
        // }
      }

  hasError(field: string, error: string) {
    const control = this.userForm.get(field);

    return control?.hasError(error) && control?.touched;
  }
  checkValidityOfField(field: string) {
    const control = this.userForm.get(field); 

    return  !control?.valid || control?.touched ;
    // return this.isFormSubmitted && !control?.valid ;

  }
  roleStatusChangeHandler() {
    if (this.userForm.get('roleStatus')?.value) {
      this.userForm.controls['portalLogin'].setValue(false);
    }
  }

  isFieldRequired() {
    const userForm = this.userForm;
    const role = userForm.get('role')?.value;
    // if (role == "") {
    //   userForm.get('role')?.setValidators([Validators.required]);
    // } else {
    //   userForm.get('role')?.clearValidators();
    // }
    // userForm.get('role')?.updateValueAndValidity();
    if (
      // userForm.get('vendor')?.value == 0 &&
      (role == RoleEnum.VendorSalesRep ||
        role == RoleEnum.VendorManager ||
        role == RoleEnum.VendorGuestUser)
    ) {
      userForm.get('vendor')?.setValidators([Validators.required]);

    } else {
      userForm.get('vendor')?.clearValidators();

    }
    userForm.get('vendor')?.updateValueAndValidity();

    if (
      // userForm.get('vendorManagerLevel')?.value == 0 &&
      role == RoleEnum.VendorManager
    ) {
      userForm.get('vendorManagerLevel')?.setValidators([Validators.required]);
    
    } else {
      userForm.get('vendorManagerLevel')?.clearValidators();
    }
    userForm.get('vendorManagerLevel')?.updateValueAndValidity();

    // if (
    //   userForm.get('reportTo')?.value == 0 &&
    //   (role == RoleEnum.VendorSalesRep ||
    //     role == RoleEnum.VendorManager ||
    //     role == RoleEnum.VendorGuestUser)
    // ) {
    //   userForm.get('reportTo')?.setValidators(Validators.required);
    // } else {
    //   userForm.get('reportTo')?.clearValidators();
    // }
    if (
      // userForm.get('relationshipManager')?.value == 0 &&
      (role == RoleEnum.VendorSalesRep ||
        role == RoleEnum.VendorManager ||
        role == RoleEnum.VendorGuestUser)
    ) {
      userForm.get('relationshipManager')?.setValidators([Validators.required]);
    } else {
      userForm.get('relationshipManager')?.clearValidators();
    }
    userForm.get('relationshipManager')?.updateValueAndValidity();
    if (
      (userForm.get('day')?.value == 0 && role == RoleEnum.VendorSalesRep) ||
      role == RoleEnum.VendorManager ||
      role == RoleEnum.VendorGuestUser
    ) {
      userForm.get('day')?.setValidators([Validators.required]);
    } else {
      userForm.get('day')?.clearValidators();
    }
    userForm.get('day')?.updateValueAndValidity();
    if (
      (userForm.get('month')?.value == 0 && role == RoleEnum.VendorSalesRep) ||
      role == RoleEnum.VendorManager ||
      role == RoleEnum.VendorGuestUser
    ) {
      userForm.get('month')?.setValidators([Validators.required]);
    } else {
      userForm.get('month')?.clearValidators();
    }
    userForm.get('month')?.updateValueAndValidity();

  }

  deleteUser(){
    const id = this._route.snapshot.params['id'];  
    const res = confirm(alertResponses.DELETION_CONFIRMATION) 
    if(res){
      if (id) {
        this._userService.deleteUser(id).subscribe((res) => {   
      },err=>console.log(err));
    }
    }
  
  }
  vendorDataSetter()  {
    const searchModel:CommonSearch ={
      pageNumber: 1,
      pageSize: Number.INT_MAX_VALUE
    }   
    
    this._vendorService.getVendors(searchModel).subscribe(res => {
      this.selectMenuVendors = [];
      res.map((e) => {
        this.selectMenuVendors.push({ option: e.name, value: e.id });
      });
    });

  }
  relationshipManagerDataSetter() {
    this._userService.getRelationshipManagers().subscribe(res=>{
      this.selectMenuRelationshipManager = [];
      res.map((e) => {
        this.selectMenuRelationshipManager.push({ option: `${e.name} ${e.surName}${e.status ? '' : ' - inactive'}`, value: e.id });
      });
    },err=>  this.selectMenuRelationshipManager = [])

  }
  managerLevelsDataSetter(id:number,){    
      this._vendorService.getManagerLevels(id).subscribe((res)=>{
        this.selectMenuManagerLevels=[];  
       res && res.map((e) => {
          this.selectMenuManagerLevels.push({  option:`${e.levelName} - Level ${e.levelNo}`, value: e.id });
        });
      },err=>  this.selectMenuManagerLevels=[])
   
    
  }
  reportingToDataSetter(vendorId:number,managerLevelId:number){
    this._userService.getReportingTo(vendorId,managerLevelId).subscribe(res=>{
      this.selectMenuReportingTo=[];
       res &&  res.map((e) => {
        this.selectMenuReportingTo.push({ option:`${e.name} ${e.surName}`, value: e.id });
      });
    },err=>  this.selectMenuReportingTo=[])    
   }
  //  ngDoCheck(): void {
  //   //Called every time that the input properties of a component or a directive are checked. Use it to extend change detection by performing a custom check.
  //   //Add 'implements DoCheck' to the class.
    
 
  //   //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
  //   //Add 'implements AfterViewInit' to the class.
  //   const role = this.userForm.get('role')?.value;
  //   if([RoleEnum.VendorGuestUser ,RoleEnum.VendorManager,RoleEnum.VendorSalesRep].includes(role)){
  //     console.log("hi from sucbeiber")
  //         this.userForm.get('role')?.valueChanges.subscribe(()=>{
  //           this.isFieldRequired();
  //         })
  //   }
  //  }
}
