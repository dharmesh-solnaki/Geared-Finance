import { formatDate } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { computeStyles } from '@popperjs/core';
import { CommonSearch } from 'src/app/Models/common-search.model';

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
import { generateRandomPassword } from 'src/app/Shared/common-functions';
import { CommonSelectmenuComponent } from 'src/app/Shared/common-selectmenu/common-selectmenu.component';

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
  selectionVendors: selectMenu[] = [];
  defaultDateSelection: selectMenu[] = [];
  defaultMonthSelection: selectMenu[] = [];
  selectMenuRelationshipManager: selectMenu[] = [];
  selectMenuManagerLevels:selectMenu[]=[]
  userForm: FormGroup = new FormGroup({});
  isEditable: boolean = false;
  userDataOnEdit: User = new User(0, '', '', '', '');
  TempPassword: string = '';


  constructor(
    private _fb: FormBuilder,
    private _userService: UserService,
    private _route: ActivatedRoute
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
      if (
        res == RoleEnum.VendorGuestUser ||
        res == RoleEnum.VendorManager ||
        res == RoleEnum.VendorSalesRep
      ) {
        this.vendorDataSetter();
        this.relationshipManagerDataSetter();
        this.userForm.controls['vendor'].valueChanges.subscribe(res=>{
          thi
        })
      }
    });
   
  }
  userFormInitializerOnEdit() {
    const formdata = this.userDataOnEdit;
    this.TempPassword = generateRandomPassword();
    this.userForm.patchValue({
      name: formdata.name,
      surname: formdata.surName,
      email: formdata.email,
      password: this.TempPassword,
      role: formdata.roleName,
      notificationPref: formdata.notificationPreferences,
      month: formdata.monthOfBirth,
      day: formdata.dayOfBirth,
      mobileNumber: formdata.mobile,
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
    });
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
      role: ['Select', Validators.required],
      vendor: [0],
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
      vendorManagerLevel: [0],
      reportTo: [0],
      relationshipManager: [0],
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
          role == RoleEnum.VendorGuestUser ||
          role == RoleEnum.VendorManager ||
          role == RoleEnum.VendorSalesRep
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
      this.isEditable ? this.userDataOnEdit.staffCode : ''
    );

    if (this.isEditable) {
      // const pass = this.TempPassword===formValues.password?null:formValues.password
      (user.password =
        this.TempPassword === formValues.password ? null : formValues.password),
        this._userService.updateUser(user).subscribe(
          (res) => {
            alert(alertResponses.UPDATE_RECORD);
          },
          (err) => {
            console.log(err);
          }
        );
    } else {
      this._userService.addUser(user).subscribe(
        (res) => {
          alert(alertResponses.UPDATE_RECORD);
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }

  hasError(field: string, error: string) {
    const control = this.userForm.get(field);

    return control?.hasError(error) && control?.touched;
  }
  checkValidityOfField(field: string) {
    const control = this.userForm.get(field);
    // return control?.invalid || control?.touched;
    return control?.hasValidator(Validators.required) || control?.touched;
  }

  roleStatusChangeHandler() {
    if (this.userForm.get('roleStatus')?.value) {
      this.userForm.controls['portalLogin'].setValue(false);
    }
  }

  isFieldRequired() {
    const userForm = this.userForm;
    const role = userForm.get('role')?.value;

    if (role == 'Select') {
      userForm.get('role')?.setValidators(Validators.required);
    } else {
      userForm.get('role')?.removeValidators(Validators.required);
    }

    if (
      userForm.get('vendor')?.value == 0 &&
      (role == RoleEnum.VendorSalesRep ||
        role == RoleEnum.VendorManager ||
        role == RoleEnum.VendorGuestUser)
    ) {
      userForm.get('vendor')?.setValidators(Validators.required);
    } else {
      userForm.get('vendor')?.removeValidators(Validators.required);
    }

    if (
      userForm.get('vendorManagerLevel')?.value == 0 &&
      role == RoleEnum.VendorManager
    ) {
      userForm.get('vendorManagerLevel')?.setValidators(Validators.required);
    } else {
      userForm.get('vendorManagerLevel')?.clearValidators();
    }

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
      userForm.get('relationshipManager')?.value == 0 &&
      (role == RoleEnum.VendorSalesRep ||
        role == RoleEnum.VendorManager ||
        role == RoleEnum.VendorGuestUser)
    ) {
      userForm.get('relationshipManager')?.setValidators(Validators.required);
    } else {
      userForm.get('relationshipManager')?.clearValidators();
    }

    if (
      (userForm.get('day')?.value == 0 && role == RoleEnum.VendorSalesRep) ||
      role == RoleEnum.VendorManager ||
      role == RoleEnum.VendorGuestUser
    ) {
      userForm.get('day')?.setValidators(Validators.required);
    } else {
      userForm.get('day')?.clearValidators();
    }
    if (
      (userForm.get('month')?.value == 0 && role == RoleEnum.VendorSalesRep) ||
      role == RoleEnum.VendorManager ||
      role == RoleEnum.VendorGuestUser
    ) {
      userForm.get('month')?.setValidators(Validators.required);
    } else {
      userForm.get('month')?.clearValidators();
    }
    userForm.updateValueAndValidity();
  }

  vendorDataSetter() {
    this._userService.getVendors().subscribe(
      (res) => {
        this.selectMenuVendors = [];
        res.map((e) => {
          this.selectMenuVendors.push({ option: e.name, value: e.id });
        });
      },
      (err) => {
        console.log(err);
      }
    );
  }
  relationshipManagerDataSetter() {
    this._userService.getRelationshipManagers().subscribe((res) => {
      this.selectMenuRelationshipManager = [];
      res.map((e) => {
        this.selectMenuRelationshipManager.push({
          option: `${e.name} ${e.surName}${e.status ? '' : ' - inactive'}`,
          value: e.id,
        });
      });
    });
  }
}
