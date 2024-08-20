import { Component } from '@angular/core';

@Component({
  selector: 'app-access-denied-page',
  // templateUrl: './access-denied-page.component.html',
  template: `<p class="text-center display-4 fw-semibold text-danger mt-5">
      Access Denied!
    </p>

    <div
      class="d-flex justify-content-center align-items-center display-5 mt-3"
    >
      <a
        href="javascript: history.go(-1)"
        role="button"
        class="btn btn-secondary m-2"
        >Go Back</a
      >
    </div> `,
})
export class AccessDeniedPageComponent {}
