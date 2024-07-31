import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { menuBarItems } from '../Shared/constants';

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['../../assets/Styles/appStyle.css'],
})
export class AppHeaderComponent implements OnInit {
  menuData: { menuItem: string; imagePath: string ,routerPath:string}[] = [];
  shuoldVisible: boolean = false;
  shouldVisibleCanvas: boolean = false;
  @Output() isNavOpned = new EventEmitter<boolean>();

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.menuData = menuBarItems;
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
}
