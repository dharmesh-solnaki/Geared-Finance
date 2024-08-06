import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { menuBarItems } from '../Shared/constants';
import { TokenService } from '../Service/token.service';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppHeaderComponent implements OnInit {
  menuData: { menuItem: string; imagePath: string ,routerPath:string}[] = [];
  shuoldVisible: boolean = false;
  shouldVisibleCanvas: boolean = false;
  userName:string='User'
  @Output() isNavOpned = new EventEmitter<boolean>();

  constructor(private _tokenService:TokenService) {
  }
  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.menuData = menuBarItems;
  }
  get getUsername():string{
  return this._tokenService.getUserNameFromToken()
  }


  navExpand() {
    const windowWidth = window.innerWidth;
    if (windowWidth <= 768) {
      this.shouldVisibleCanvas = true; // Show offcanvas menu
      this.shuoldVisible = false; // Hide other elements as needed
    } else {
      this.shouldVisibleCanvas = false; // Hide offcanvas menu
      this.shuoldVisible = !this.shuoldVisible; // Toggle other elements
    }
   this.isNavOpned.emit(this.shuoldVisible);
  }
  onLogout(){
    this._tokenService.clearToken();
  }
}
