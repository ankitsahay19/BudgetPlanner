import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiEndpoints } from '../core/constants/api-endpoints';
import { WishlistModel } from '../models/WishlistModel';

@Injectable({ providedIn: 'root' })
export class WishlistService {

  constructor(private http: HttpClient) {}

  // ✅ Create or Edit wishlist item — wrapped inside { wishList: item }

saveWishListItem(item: WishlistModel): Observable<WishlistModel> {
  // Send item directly — no wrapping inside wishList property
  return this.http.post<WishlistModel>(ApiEndpoints.WishLists.createOrEdit, item);
}

  // Get all wishlist items
  getWishLists(): Observable<WishlistModel[]> {
    return this.http.get<WishlistModel[]>(ApiEndpoints.WishLists.getAll);
  }

  // Get a single wishlist item by id
  getWishListItem(id: number): Observable<WishlistModel> {
    return this.http.get<WishlistModel>(ApiEndpoints.WishLists.getById(id));
  }

  // Delete a wishlist item by id
  deleteWishListItem(id: number): Observable<void> {
    return this.http.delete<void>(ApiEndpoints.WishLists.delete(id));
  }
}
